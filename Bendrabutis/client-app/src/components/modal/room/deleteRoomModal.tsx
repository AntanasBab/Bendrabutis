import React from "react";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import Modal from "@mui/material/Modal";
import { DormFloor, Dormitory, Room } from "../../../data/dataModels";
import Button from "@mui/material/Button";
import CancelIcon from "@mui/icons-material/Cancel";
import Stack from "@mui/material/Stack";
import * as yup from "yup";
import DeleteIcon from "@mui/icons-material/Delete";
import Cookies from "universal-cookie";
import axios from "axios";
import { UrlManager } from "../../../utils/urlmanager";

export interface DeleteRoomModalProps {
  open: boolean;
  onClose: () => void;
  room?: Room;
}

const schema = yup
  .object({
    number: yup.number().required("Įveskite skaičių"),
  })
  .required("Privaloma");

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

const DeleteRoomModal = (props: DeleteRoomModalProps) => {
  const cookies = new Cookies();
  const onDelete = (id?: number) => {
    if (id)
      axios
        .delete(UrlManager.getDeleteRoomEndpoint(id), {
          headers: {
            Authorization: `Bearer ${cookies.get("JWT")}`,
          },
        })
        .then(() => {
          window.location.reload();
        })
        .catch((err) => {});
  };

  return (
    <Modal
      open={props.open}
      onClose={() => props.onClose()}
      aria-labelledby="modal-modal-title"
      aria-describedby="modal-modal-description"
    >
      <Box sx={style}>
        <Typography
          id="modal-modal-title"
          variant="h6"
          component="h2"
          className="mb-6"
        >
          {`Ar tikrai norite ištrinti - "${props.room?.number} kambarį"?`}
        </Typography>
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
            startIcon={<DeleteIcon />}
            onClick={() => onDelete(props.room?.id)}
          >
            Ištrinti
          </Button>
        </Stack>
      </Box>
    </Modal>
  );
};

export default DeleteRoomModal;
