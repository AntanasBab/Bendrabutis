import {
  Typography,
  Grid,
  Card,
  CardActionArea,
  CardContent,
  CardMedia,
  Stack,
  Button,
} from "@mui/material";
import axios from "axios";
import React, { useEffect, useState } from "react";
import ResponsiveAppBar from "../../components/header/ResponsiveAppBar";
import { DormFloor } from "../../data/dataModels";
import { UrlManager } from "../../utils/urlmanager";
import DeleteIcon from "@mui/icons-material/Delete";
import EditIcon from "@mui/icons-material/Edit";
import Footer from "../../components/footer/Footer";
import { FloorForm } from "./FloorForm";
import Cookies from "universal-cookie";
import UpdateFloorModal from "../../components/modal/floor/updateFloorModal";
import DeleteFloorModal from "../../components/modal/floor/deleteFloorModal";

const FloorManagement = (): JSX.Element => {
  const cookies = new Cookies();
  const [floorList, setFloorList] = useState<DormFloor[]>();
  const [deleteModalOpen, setDeleteModalOpen] = React.useState(false);
  const [updateModalOpen, setUpdateModalOpen] = React.useState(false);

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
            {floorList?.map((floor, index) => (
              <Grid item key={index} xs={"auto"} className="mt-1">
                <Card sx={{ maxWidth: 345 }}>
                  <CardActionArea>
                    <CardMedia
                      component="img"
                      height="140"
                      image="https://thinkwood-wordpress.s3.amazonaws.com/wp-content/uploads/2020/09/22162304/ThinkWood-Hybrid-BuildingWhite-F-rev.jpg"
                      alt="Aukstas"
                    />
                    <CardContent>
                      <Typography gutterBottom variant="h5" component="div">
                        {floor.number} aukštas
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
                        <DeleteFloorModal
                          floor={floor}
                          onClose={() => setDeleteModalOpen(false)}
                          open={deleteModalOpen}
                        />
                        <Button
                          variant="contained"
                          endIcon={<EditIcon />}
                          onClick={() => setUpdateModalOpen(true)}
                        >
                          Redaguoti
                        </Button>
                        <UpdateFloorModal
                          open={updateModalOpen}
                          floor={floor}
                          onClose={() => setUpdateModalOpen(false)}
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
