import {
  Typography,
  Grid,
  Card,
  CardActionArea,
  CardContent,
  CardMedia,
  Stack,
} from "@mui/material";
import axios from "axios";
import React, { useEffect, useState } from "react";
import ResponsiveAppBar from "../../components/header/ResponsiveAppBar";
import { DormFloor } from "../../data/dataModels";
import { UrlManager } from "../../utils/urlmanager";
// import DeleteIcon from "@mui/icons-material/Delete";
// import EditIcon from "@mui/icons-material/Edit";
// import DeleteDormModal from "../../components/modal/deleteDormModal";
// import UpdateDormModal from "../../components/modal/updateDormModal";
import Footer from "../../components/footer/Footer";
import { FloorForm } from "./FloorForm";
import Cookies from "universal-cookie";

const FloorManagement = (): JSX.Element => {
  const cookies = new Cookies();
  const [floorList, setFloorList] = useState<DormFloor[]>();
  // const [deleteModalOpen, setDeleteModalOpen] = React.useState(false);
  // const [updateModalOpen, setUpdateModalOpen] = React.useState(false);

  useEffect(() => {
    axios
      .get<DormFloor[]>(UrlManager.getAllDormFloorsEndpoint(), {
        headers: { Authorization: `Bearer ${cookies.get("JWT")}` },
      })
      .then((floors) => {
        setFloorList(floors.data);
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
            {floorList?.map((floor, index) => (
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
                        Aukštas {floor.number}
                      </Typography>
                      <Stack
                        direction="row"
                        spacing={2}
                        className={"flex mt-6"}
                      >
                        {/* <Button
                          variant="outlined"
                          startIcon={<DeleteIcon />}
                          onClick={() => setDeleteModalOpen(true)}
                        >
                          Panaikinti
                        </Button> */}
                        {/* <DeleteDormModal
                          open={deleteModalOpen}
                          onClose={() => setDeleteModalOpen(false)}
                          dorm={floorList[index]}
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
                        /> */}
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
            Naujas aukštas:
          </Typography>
          <FloorForm />
        </Grid>
      </Grid>
      <Footer />
    </>
  );
};

export default FloorManagement;
