import React from "react";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import Modal from "@mui/material/Modal";
import { UserRequest } from "../../data/dataModels";
import Button from "@mui/material/Button";
import DoneIcon from "@mui/icons-material/Done";
import CancelIcon from "@mui/icons-material/Cancel";
import Stack from "@mui/material/Stack";
import { TextField } from "@mui/material";
import * as yup from "yup";
import { useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";

export interface UpdateRequestModalProps {
  open: boolean;
  onClose: () => void;
  request: UserRequest;
}

const schema = yup
  .object({
    description: yup.string().required("Laukas negali būti tusčias."),
  })
  .required("Laukas negali būti tusčias.");

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
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm({
    resolver: yupResolver(schema),
  });

  const onSubmit = (data: any) => console.log(data);
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
        <form onSubmit={handleSubmit(onSubmit)}>
          <TextField
            label="Prašymo žinutė"
            style={{ margin: "10px 0 0 0" }}
            placeholder="Prašymo žinutė"
            {...register("description")}
            defaultValue={props.request.description}
            error={!!errors.description}
            helperText={errors.description?.message}
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
            <Button variant="contained" startIcon={<DoneIcon />}>
              Atnaujinti
            </Button>
          </Stack>
        </form>
      </Box>
    </Modal>
  );
};

export default UpdateRequestModal;
