import {
  Typography,
  Grid,
  Card,
  CardActionArea,
  CardContent,
  CardMedia,
  Button,
  Stack,
} from "@mui/material";
import axios from "axios";
import React, { useEffect, useState } from "react";
import ResponsiveAppBar from "../../components/header/ResponsiveAppBar";
import { UrlManager } from "../../utils/urlmanager";
import DeleteIcon from "@mui/icons-material/Delete";
import EditIcon from "@mui/icons-material/Edit";
import DeleteDormModal from "../../components/modal/dorm/deleteDormModal";
import UpdateDormModal from "../../components/modal/dorm/updateDormModal";
import Footer from "../../components/footer/Footer";
import { DormForm } from "./DormForm";
import { Dormitory } from "../../data/dataModels";
import Cookies from "universal-cookie";

const DormManagement = (): JSX.Element => {
  const cookies = new Cookies();
  const [dormList, SetDormList] = useState<Dormitory[]>();
  const [selectedDorm, setselectedDorm] = useState<Dormitory>();
  const [deleteModalOpen, setDeleteModalOpen] = React.useState(false);
  const [updateModalOpen, setUpdateModalOpen] = React.useState(false);

  useEffect(() => {
    axios.get<Dormitory[]>(UrlManager.getAllDormsEndpoint()).then((dorms) => {
      SetDormList(dorms.data);
    });
  }, []);

  return (
    <>
      <ResponsiveAppBar />
      <Grid
        container
        spacing={2}
        justifyContent="flex-start"
        alignItems="center"
        sx={{ m: 2, width: "90%" }}
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
              <Grid item key={index} xs={"auto"}>
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
                          onClick={() => {
                            setselectedDorm(dorm);
                            setDeleteModalOpen(true);
                          }}
                        >
                          Panaikinti
                        </Button>
                        <Button
                          variant="contained"
                          endIcon={<EditIcon />}
                          onClick={() => {
                            setselectedDorm(dorm);
                            setUpdateModalOpen(true);
                          }}
                        >
                          Redaguoti
                        </Button>
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
          <DormForm />
        </Grid>
      </Grid>
      <DeleteDormModal
        open={deleteModalOpen}
        onClose={() => setDeleteModalOpen(false)}
        dorm={selectedDorm}
      />
      <UpdateDormModal
        open={updateModalOpen}
        onClose={() => setUpdateModalOpen(false)}
        dorm={selectedDorm}
      />
      <Footer />
    </>
  );
};

export default DormManagement;
