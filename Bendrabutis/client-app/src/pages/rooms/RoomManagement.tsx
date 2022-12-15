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
import { Room } from "../../data/dataModels";
import { UrlManager } from "../../utils/urlmanager";
import DeleteIcon from "@mui/icons-material/Delete";
import EditIcon from "@mui/icons-material/Edit";
import Footer from "../../components/footer/Footer";
import { RoomForm } from "./RoomForm";
import Cookies from "universal-cookie";
import DeleteRoomModal from "../../components/modal/room/deleteRoomModal";
import UpdateRoomModal from "../../components/modal/room/updateRoomModal";

const RoomManagement = (): JSX.Element => {
  const cookies = new Cookies();
  const [roomList, setRoomList] = useState<Room[]>();
  const [deleteModalOpen, setDeleteModalOpen] = useState(false);
  const [updateModalOpen, setUpdateModalOpen] = useState(false);
  const [selectedRoom, setSelectedRoom] = useState<Room>();

  useEffect(() => {
    axios
      .get<Room[]>(UrlManager.getRoomsEndpoint(), {
        headers: { Authorization: `Bearer ${cookies.get("JWT")}` },
      })
      .then((rooms) => {
        setRoomList(rooms.data);
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
            {roomList?.map((room) => (
              <Grid item xs={"auto"}>
                <Card sx={{ maxWidth: 345 }}>
                  <CardActionArea>
                    <CardMedia
                      component="img"
                      height="140"
                      image="https://images.pexels.com/photos/1457842/pexels-photo-1457842.jpeg?cs=srgb&dl=pexels-jean-van-der-meulen-1457842.jpg&fm=jpg"
                      alt="Bendrabutis"
                    />
                    <CardContent>
                      <Typography gutterBottom variant="h5" component="div">
                        {`Kambario numeris: ${room.number}`}
                      </Typography>
                      <Typography variant="body1" color="text.secondary">
                        {`Kambario plotas: ${room.area}`}
                      </Typography>
                      <Typography variant="body1" color="text.secondary">
                        {`Vietų skaičius: ${room.numberOfLivingPlaces}`}
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
                            setSelectedRoom(room);
                            setDeleteModalOpen(true);
                          }}
                        >
                          Panaikinti
                        </Button>
                        <Button
                          variant="contained"
                          endIcon={<EditIcon />}
                          onClick={() => {
                            setSelectedRoom(room);
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
            <DeleteRoomModal
              room={selectedRoom}
              onClose={() => setDeleteModalOpen(false)}
              open={deleteModalOpen}
            />
            <UpdateRoomModal
              room={selectedRoom}
              onClose={() => setUpdateModalOpen(false)}
              open={updateModalOpen}
            />
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
            Naujas kambarys:
          </Typography>
          <RoomForm />
        </Grid>
      </Grid>
      <Footer />
    </>
  );
};

export default RoomManagement;
