import React, { useState } from "react";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import Modal from "@mui/material/Modal";
import { UserRequest } from "../../../data/dataModels";
import Button from "@mui/material/Button";
import DoneIcon from "@mui/icons-material/Done";
import CancelIcon from "@mui/icons-material/Cancel";
import Stack from "@mui/material/Stack";
import { TextField } from "@mui/material";
import axios from "axios";
import { UrlManager } from "../../../utils/urlmanager";
import Cookies from "universal-cookie";

export interface UpdateRequestModalProps {
  open: boolean;
  onClose: () => void;
  request?: UserRequest;
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

const UpdateRequestModal = (props: UpdateRequestModalProps) => {
  const cookies = new Cookies();
  const [description, setDescription] = useState<string | undefined>(
    props.request?.description
  );

  const onUpdate = () => {
    if (props.request && description)
      axios
        .patch(
          UrlManager.getUpdateRequestEndpoint(props.request.id, description),
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
          Prašymo redagavimas:
        </Typography>
        <form>
          <TextField
            label="Prašymo žinutė"
            style={{ margin: "10px 0 0 0" }}
            placeholder="Prašymo žinutė"
            defaultValue={props.request?.description}
            onChange={(e) => setDescription(e.target.value)}
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

export default UpdateRequestModal;
