import {
  Typography,
  Grid,
  Card,
  CardActionArea,
  CardContent,
  CardMedia,
  Button,
  Stack,
  TextField,
} from "@mui/material";
import axios from "axios";
import React, { useEffect, useState } from "react";
import ResponsiveAppBar from "../../components/header/ResponsiveAppBar";
import { Dormitory } from "../../data/dataModels";
import { UrlManager } from "../../utils/urlmanager";
import DeleteIcon from "@mui/icons-material/Delete";
import EditIcon from "@mui/icons-material/Edit";
import * as yup from "yup";
import { useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import DeleteDormModal from "../../components/modal/deleteDormModal";
import UpdateDormModal from "../../components/modal/updateDormModal";
import Footer from "../../components/footer/Footer";

const schema = yup
  .object({
    name: yup.string().required("Parasyk"),
    address: yup.string().required("Parasyk"),
    roomCapacity: yup.number().required("Parasyk"),
  })
  .required("UZPILDYK");

const DormManagement = (): JSX.Element => {
  const [dormList, SetDormList] = useState<Dormitory[]>();
  const [deleteModalOpen, setDeleteModalOpen] = React.useState(false);
  const [updateModalOpen, setUpdateModalOpen] = React.useState(false);

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm({
    resolver: yupResolver(schema),
  });

  const onSubmit = (data: any) => console.log(data);
  const btnstyle = { margin: "15px 0 15px 0" };

  useEffect(() => {
    axios.get<Dormitory[]>(UrlManager.getAllDormsEndpoint()).then((dorms) => {
      SetDormList(dorms.data);
    });
  }, []);

  return (
    <>
      <ResponsiveAppBar />
      <Grid
        className="mt-3"
        container
        direction="row"
        justifyContent="flex-start"
        alignItems="center"
      >
        <Grid item md={6}>
          <Grid
            container
            direction="row"
            justifyContent="flex-start"
            alignItems="flex-start"
            spacing={1}
          >
            {dormList?.map((dorm, index) => (
              <Grid item key={index} xs={"auto"} className="mt-1">
                <Card sx={{ maxWidth: 345 }}>
                  <CardActionArea>
                    <CardMedia
                      component="img"
                      height="140"
                      image="https://dormitory.ktu.edu/wp-content/uploads/sites/244/2018/07/Bendrabutis.png"
                      alt="Bendrabutis"
                    />
                    <CardContent>
                      <Typography gutterBottom variant="h5" component="div">
                        {dorm.name}
                      </Typography>
                      <Typography variant="body1" color="text.secondary">
                        {dorm.address}
                      </Typography>
                      <Typography variant="body1" color="text.secondary">
                        {`Iš viso kambarių: ${dorm.roomCapacity}`}
                      </Typography>
                      <Stack
                        direction="row"
                        spacing={2}
                        className={"flex mt-6"}
                      >
                        <Button
                          variant="outlined"
                          startIcon={<DeleteIcon />}
                          onClick={() => setDeleteModalOpen(true)}
                        >
                          Panaikinti
                        </Button>
                        <DeleteDormModal
                          open={deleteModalOpen}
                          onClose={() => setDeleteModalOpen(false)}
                          dorm={dormList[index]}
                        />
                        <Button
                          variant="contained"
                          endIcon={<EditIcon />}
                          onClick={() => setUpdateModalOpen(true)}
                        >
                          Redaguoti
                        </Button>
                        <UpdateDormModal
                          open={updateModalOpen}
                          onClose={() => setUpdateModalOpen(false)}
                          dorm={dorm}
                        />
                      </Stack>
                    </CardContent>
                  </CardActionArea>
                </Card>
              </Grid>
            ))}
          </Grid>
        </Grid>
        <Grid
          item
          md={6}
          direction="row"
          justifyContent="flex-start"
          alignItems="flex-start"
        >
          <Typography gutterBottom variant="h5" component="div">
            Naujas bendrabutis:
          </Typography>
          <form onSubmit={handleSubmit(onSubmit)}>
            <TextField
              label="Bendrabučio pavadinimas"
              style={{ margin: "10px 0 0 0" }}
              placeholder="Įveskite bendrabučio pavadinimas"
              {...register("name")}
              error={!!errors.name}
              helperText={errors.name?.message}
              fullWidth
              required
            />
            <TextField
              label="Adresas"
              placeholder="Įveskite adresą"
              {...register("address")}
              error={!!errors.address}
              style={{ margin: "10px 0 0 0" }}
              helperText={errors.address?.message}
              fullWidth
              required
            />
            <TextField
              label="Kambarių kiekis"
              placeholder="Įveskite kambarių kiekį"
              {...register("roomCapacity")}
              style={{ margin: "10px 0 0 0" }}
              error={!!errors.roomCapacity}
              helperText={errors.roomCapacity?.message}
              fullWidth
              required
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

export default DormManagement;
