import {
  Typography,
  Grid,
  Button,
  TextField,
  InputLabel,
  Select,
  Card,
  CardActionArea,
  CardContent,
  Stack,
  MenuItem,
} from "@mui/material";
import ResponsiveAppBar from "../../components/header/ResponsiveAppBar";
import * as yup from "yup";
import { useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import Footer from "../../components/footer/Footer";
import { useEffect, useState } from "react";
import { SelectChangeEvent } from "@mui/material/Select";
import { RequestTypes, UserRequest } from "../../data/dataModels";
import axios from "axios";
import { UrlManager } from "../../utils/urlmanager";
import Cookies from "universal-cookie";
import UpdateRequestModal from "../../components/modal/updateRequestModal";
import EditIcon from "@mui/icons-material/Edit";

const schema = yup
  .object({
    text: yup.string().required("Įrašykite tekstą"),
  })
  .required("Forma negali būti tusčia");

export const Requests = (): JSX.Element => {
  const cookies = new Cookies();
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm({
    resolver: yupResolver(schema),
  });

  const [reqType, setReqType] = useState<RequestTypes>(RequestTypes.ChangeRoom);
  const [reqList, setReqList] = useState<UserRequest[]>([]);
  const [updateModalOpen, setUpdateModalOpen] = useState<boolean>(false);

  useEffect(() => {
    axios
      .get<UserRequest[]>(UrlManager.geAllRoomsEndpoint(), {
        headers: { Authorization: `Bearer ${cookies.get("JWT")}` },
      })
      .then((floors) => {
        setReqList(floors.data);
      });
  });

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
        direction="row"
        justifyContent="flex-start"
        spacing={3}
        alignItems="center"
      >
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
        <Grid item md={6} xl>
          <Typography
            gutterBottom
            variant="h4"
            component="div"
            display="flex"
            justifyContent="flex-start"
          >
            Mano prašymai
          </Typography>
          {reqList.length === 0 ? (
            <>
              <Typography
                gutterBottom
                variant="h6"
                component="div"
                display="flex"
                justifyContent="flex-start"
              >
                Jūs neturite nei vieno prašymo.
              </Typography>
              <Typography
                gutterBottom
                variant="h6"
                component="div"
                display="flex"
                justifyContent="flex-start"
              >
                Norint pateikti naują prašyma, užpildykite praymo formą kairėje.
              </Typography>
            </>
          ) : (
            <>
              {reqList.map((req, index) => (
                <Grid item key={index} xs={"auto"} className="mt-1">
                  <Card sx={{ maxWidth: 345 }}>
                    <CardActionArea>
                      <CardContent>
                        <Typography gutterBottom variant="h3" component="div">
                          {req.requestType}
                        </Typography>
                        <Typography gutterBottom variant="h5" component="div">
                          {req.description}
                        </Typography>
                        <Stack
                          direction="row"
                          spacing={2}
                          className={"flex mt-6"}
                        >
                          <Button
                            variant="contained"
                            endIcon={<EditIcon />}
                            onClick={() => setUpdateModalOpen(true)}
                          >
                            Redaguoti
                          </Button>
                          <UpdateRequestModal
                            request={req}
                            open={updateModalOpen}
                            onClose={() => setUpdateModalOpen(false)}
                          />
                        </Stack>
                      </CardContent>
                    </CardActionArea>
                  </Card>
                </Grid>
              ))}
            </>
          )}
        </Grid>
      </Grid>
      <Footer />
    </>
  );
};
