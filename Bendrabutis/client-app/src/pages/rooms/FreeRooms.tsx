import {
  MenuItem,
  Typography,
  Grid,
  Card,
  CardActionArea,
  CardContent,
  CardMedia,
} from "@mui/material";
import axios from "axios";
import * as React from "react";
import { useEffect, useState } from "react";
import Footer from "../../components/footer/Footer";
import ResponsiveAppBar from "../../components/header/ResponsiveAppBar";
import { Dormitory, Room } from "../../data/dataModels";
import { UrlManager } from "../../utils/urlmanager";

export default function Rooms() {
  const [dormList, SetDormList] = useState<Dormitory[]>();
  const [roomList, SetRoomList] = useState<Room[]>();
  const [selectedDorm, SetSelectedDorm] = useState<number>(0);

  useEffect(() => {
    axios.get<Dormitory[]>(UrlManager.getAllDormsEndpoint()).then((dorms) => {
      SetDormList(dorms.data);
    });
  }, []);

  useEffect(() => {
    if (selectedDorm !== 0)
      axios
        .get<Room[]>(UrlManager.getFreeRoomsEndpoint(selectedDorm))
        .then((rooms) => {
          SetRoomList(rooms.data);
          console.log(rooms.data);
        })
        .catch(() => {
          SetRoomList(undefined);
        });
  }, [selectedDorm]);

  return (
    <>
      <ResponsiveAppBar />
      <Grid container spacing={2}>
        <Grid item md={6} spacing={2}>
          {dormList &&
            dormList.map((dorm) => (
              <Card
                className="mt-2 ml-2"
                sx={{ maxWidth: 400 }}
                onClick={() => SetSelectedDorm(dorm.id)}
              >
                <CardActionArea>
                  <CardMedia
                    component="img"
                    height="140"
                    image="https://dormitory.ktu.edu/wp-content/uploads/sites/244/2018/07/Bendrabutis.png"
                    alt="barakas"
                  />
                  <CardContent>
                    <Typography gutterBottom variant="h5" component="div">
                      {dorm.name}
                    </Typography>
                    <Typography variant="body2" color="text.secondary">
                      {`Laisvų vietų: ${dorm.address}`}
                    </Typography>
                  </CardContent>
                </CardActionArea>
              </Card>
            ))}
        </Grid>
        <Grid item md={6} spacing={2}>
          {roomList &&
            roomList.map((room, key) => (
              <Card sx={{ maxWidth: 250 }} className="mt-2 ml-2">
                <CardActionArea>
                  <CardMedia
                    component="img"
                    height="140"
                    image="https://images.pexels.com/photos/1457842/pexels-photo-1457842.jpeg?cs=srgb&dl=pexels-jean-van-der-meulen-1457842.jpg&fm=jpg"
                    alt="barakas"
                  />
                  <CardContent>
                    <Typography gutterBottom variant="h5" component="div">
                      {`Kambarys - ${room.number}`}
                    </Typography>
                    <Typography variant="body2" color="text.secondary">
                      {`Laisvų vietų: ${room.numberOfLivingPlaces}`}
                    </Typography>
                  </CardContent>
                </CardActionArea>
              </Card>
            ))}
        </Grid>
      </Grid>
      <Footer />
    </>
  );
}
