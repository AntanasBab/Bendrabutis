import {
  Typography,
  Grid,
  Button,
  TextField,
  InputLabel,
  Select,
} from "@mui/material";
import ResponsiveAppBar from "../../components/header/ResponsiveAppBar";
import * as yup from "yup";
import { useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import Footer from "../../components/footer/Footer";
import { useState } from "react";
import MenuItem from "@mui/material/MenuItem";
import { SelectChangeEvent } from "@mui/material/Select";
import { RequestTypes } from "../../data/dataModels";

const schema = yup
  .object({
    text: yup.string().required("Įrašykite tekstą"),
  })
  .required("Forma negali būti tusčia");

export const Requests = (): JSX.Element => {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm({
    resolver: yupResolver(schema),
  });

  const [reqType, setReqType] = useState<RequestTypes>(RequestTypes.ChangeRoom);

  const handleChange = (event: SelectChangeEvent) => {
    switch (event.target.value) {
      case "3":
        setReqType(RequestTypes.ChangeRoom);
        break;
      case "1":
        setReqType(RequestTypes.MoveIn);
        break;
      case "2":
        setReqType(RequestTypes.MoveOut);
        break;
      default:
        setReqType(RequestTypes.None);
        break;
    }
  };
  const onSubmit = (data: any) => console.log(data);
  const btnstyle = { margin: "15px 0 15px 0" };

  return (
    <>
      <ResponsiveAppBar />
      <Grid
        container
        className="mt-3"
        direction="row"
        justifyContent="flex-start"
        alignItems="center"
      >
        <Grid item md={6}>
          <Typography gutterBottom variant="h5" component="div">
            Mano prašymai
          </Typography>
        </Grid>
        <Grid item md={6} justifyContent="flex-start" alignItems="flex-start">
          <Typography gutterBottom variant="h5" component="div">
            Naujas prašymas:
          </Typography>
          <form onSubmit={handleSubmit(onSubmit)}>
            <InputLabel id="demo-simple-select-label">Prašymo tipas</InputLabel>
            <Select
              labelId="demo-simple-select-label"
              id="demo-simple-select"
              value={reqType.toString()}
              label="Prašymo tipas"
              onChange={handleChange}
            >
              <MenuItem value={"3"}>Keisti kambarį</MenuItem>
              <MenuItem value={"1"}>Įsikelti</MenuItem>
              <MenuItem value={"2"}>Išsikelti</MenuItem>
            </Select>
            <TextField
              id="outlined-textarea"
              label="Įrašykite prašymo tekstą"
              placeholder="Tekstas"
              style={{ margin: "10px 0 0 0" }}
              {...register("text")}
              error={!!errors.text}
              helperText={errors.text?.message}
              fullWidth
              multiline
            />
            <Button
              type="submit"
              color="primary"
              variant="contained"
              style={btnstyle}
              fullWidth
            >
              Sukurti
            </Button>
          </form>
        </Grid>
      </Grid>
      <Footer />
    </>
  );
};
