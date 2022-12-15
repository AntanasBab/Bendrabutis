import React, { useState } from "react";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import Modal from "@mui/material/Modal";
import { Room } from "../../../data/dataModels";
import Button from "@mui/material/Button";
import DoneIcon from "@mui/icons-material/Done";
import CancelIcon from "@mui/icons-material/Cancel";
import Stack from "@mui/material/Stack";
import { TextField } from "@mui/material";
import axios from "axios";
import { UrlManager } from "../../../utils/urlmanager";
import Cookies from "universal-cookie";

export interface UpdateRoomModalProps {
  open: boolean;
  onClose: () => void;
  room?: Room;
}
const style = {
  position: "absolute" as "absolute",
  top: "50%",
  left: "50%",
  transform: "translate(-50%, -50%)",
  width: 400,
  bgcolor: "background.paper",
  border: "2px solid #000",
  boxShadow: 24,
  p: 4,
};

const UpdateRoomModal = (props: UpdateRoomModalProps) => {
  const [number, setNumber] = useState<number | undefined>(props.room?.number);
  const [area, setArea] = useState<number | undefined>(props.room?.area);
  const [numberOfLivingPlaces, setNumberOfLivingPlaces] = useState<
    number | undefined
  >(props.room?.numberOfLivingPlaces);

  const cookies = new Cookies();

  const onUpdate = () => {
    if (props.room && number && numberOfLivingPlaces && area)
      axios
        .patch(
          UrlManager.getUpdateRoomEndpoint(
            props.room?.id,
            number,
            numberOfLivingPlaces,
            area
          ),
          {},
          {
            headers: {
              Authorization: `Bearer ${cookies.get("JWT")}`,
            },
          }
        )
        .then(() => window.location.reload());
  };

  return (
    <Modal
      open={props.open}
      onClose={() => props.onClose()}
      aria-labelledby="modal-modal-title"
      aria-describedby="modal-modal-description"
    >
      <Box sx={style}>
        <Typography gutterBottom variant="h5" component="div">
          Bendrabučio redagavimas:
        </Typography>
        <form>
          <TextField
            label="Kambario numeris"
            style={{ margin: "10px 0 0 0" }}
            placeholder="Įveskite kamabrio numerį"
            defaultValue={props.room?.number}
            onChange={(e) => setNumber(Number(e.target.value))}
            fullWidth
            required
          />
          <TextField
            label="Kambario plotas"
            placeholder="Įveskite kambario plotą"
            style={{ margin: "10px 0 0 0" }}
            defaultValue={props.room?.area}
            onChange={(e) => setArea(Number(e.target.value))}
            fullWidth
            required
          />
          <TextField
            label="Kambario vietų skaičius"
            placeholder="Įveskite kambario vietų skaičių"
            style={{ margin: "10px 0 0 0" }}
            defaultValue={props.room?.numberOfLivingPlaces}
            onChange={(e) => setNumberOfLivingPlaces(Number(e.target.value))}
            fullWidth
            required
          />
          <Stack
            direction="row"
            spacing={2}
            justifyContent="space-between"
            className="mt-5"
          >
            <Button
              variant="outlined"
              startIcon={<CancelIcon />}
              onClick={() => props.onClose()}
            >
              Atgal
            </Button>
            <Button
              variant="contained"
              startIcon={<DoneIcon />}
              onClick={() => onUpdate()}
            >
              Atnaujinti
            </Button>
          </Stack>
        </form>
      </Box>
    </Modal>
  );
};

export default UpdateRoomModal;
