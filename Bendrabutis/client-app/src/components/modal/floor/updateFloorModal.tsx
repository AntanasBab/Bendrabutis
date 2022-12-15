import React, { useState } from "react";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import Modal from "@mui/material/Modal";
import { DormFloor } from "../../../data/dataModels";
import Button from "@mui/material/Button";
import DoneIcon from "@mui/icons-material/Done";
import CancelIcon from "@mui/icons-material/Cancel";
import Stack from "@mui/material/Stack";
import { TextField } from "@mui/material";
import axios from "axios";
import { UrlManager } from "../../../utils/urlmanager";
import Cookies from "universal-cookie";

export interface UpdateFloorModalProps {
  open: boolean;
  onClose: () => void;
  floor: DormFloor;
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

const UpdateFloorModal = (props: UpdateFloorModalProps) => {
  const [number, setNumber] = useState<number | undefined>(props.floor?.number);

  const cookies = new Cookies();
  const onUpdate = () => {
    if (props.floor && number)
      axios
        .patch(
          UrlManager.getUpdateFloorEndpoint(props.floor?.id, number),
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
          Aukšto redagavimas:
        </Typography>
        <form>
          <TextField
            label="Aukšto skaičius"
            style={{ margin: "10px 0 0 0" }}
            placeholder="Įveskite aukšto skaičių"
            defaultValue={props.floor.number}
            onChange={(e) => setNumber(Number(e.target.value))}
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

export default UpdateFloorModal;
