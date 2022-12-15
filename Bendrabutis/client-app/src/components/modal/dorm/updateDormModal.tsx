import React, { useState } from "react";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import Modal from "@mui/material/Modal";
import Button from "@mui/material/Button";
import DoneIcon from "@mui/icons-material/Done";
import CancelIcon from "@mui/icons-material/Cancel";
import Stack from "@mui/material/Stack";
import { TextField } from "@mui/material";
import { Dormitory } from "../../../data/dataModels";
import Cookies from "universal-cookie";
import axios from "axios";
import { UrlManager } from "../../../utils/urlmanager";

export interface UpdateDormModalProps {
  open: boolean;
  onClose: () => void;
  dorm?: Dormitory;
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

const UpdateDormModal = (props: UpdateDormModalProps) => {
  const [name, setName] = useState<string | undefined>(props.dorm?.name);
  const [adress, setAdress] = useState<string | undefined>(props.dorm?.address);
  const [roomCapacity, setRoomCapacity] = useState<number | undefined>(
    props.dorm?.roomCapacity
  );

  const cookies = new Cookies();
  const onUpdate = () => {
    if (props.dorm && name && adress && roomCapacity)
      axios
        .patch(
          UrlManager.getUpdateDormEndpoint(
            props.dorm?.id,
            name,
            adress,
            roomCapacity
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
            label="Bendrabučio pavadinimas"
            style={{ margin: "10px 0 0 0" }}
            placeholder="Įveskite bendrabučio pavadinimas"
            defaultValue={props.dorm?.name}
            onChange={(e) => setName(e.target.value)}
            fullWidth
            required
          />
          <TextField
            label="Adresas"
            placeholder="Įveskite adresą"
            style={{ margin: "10px 0 0 0" }}
            defaultValue={props.dorm?.address}
            onChange={(e) => setAdress(e.target.value)}
            fullWidth
            required
          />
          <TextField
            label="Kambarių kiekis"
            placeholder="Įveskite kambarių kiekį"
            style={{ margin: "10px 0 0 0" }}
            defaultValue={props.dorm?.roomCapacity}
            onChange={(e) => setRoomCapacity(Number(e.target.value))}
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

export default UpdateDormModal;
