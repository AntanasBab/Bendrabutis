import React from "react";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import Modal from "@mui/material/Modal";
import { Dormitory } from "../../data/dataModels";
import Button from "@mui/material/Button";
import DoneIcon from "@mui/icons-material/Done";
import CancelIcon from "@mui/icons-material/Cancel";
import Stack from "@mui/material/Stack";
import { TextField } from "@mui/material";
import * as yup from "yup";
import { useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";

export interface UpdateDormModalProps {
  open: boolean;
  onClose: () => void;
  dorm: Dormitory;
}

//TODO: FIX VALIDATION
const schema = yup
  .object({
    name: yup.string().default("sss").required("Parasyk"),
    address: yup.string().required("Parasyk"),
    roomCapacity: yup.number().required("Parasyk"),
  })
  .required("UZPILDYK");

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
          Bendrabučio redagavimas:
        </Typography>
        <form onSubmit={handleSubmit(onSubmit)}>
          <TextField
            label="Bendrabučio pavadinimas"
            style={{ margin: "10px 0 0 0" }}
            placeholder="Įveskite bendrabučio pavadinimas"
            {...register("name")}
            defaultValue={props.dorm.name}
            error={!!errors.name}
            helperText={errors.name?.message}
            fullWidth
            required
          />
          <TextField
            label="Adresas"
            placeholder="Įveskite adresą"
            {...register("address")}
            defaultValue={props.dorm.address}
            error={!!errors.address}
            style={{ margin: "10px 0 0 0" }}
            helperText={errors.address?.message}
            fullWidth
            required
          />
          <TextField
            label="Kambarių kiekis"
            placeholder="Įveskite kambarių kiekį"
            {...register("roomCapacity")}
            style={{ margin: "10px 0 0 0" }}
            defaultValue={props.dorm.roomCapacity}
            error={!!errors.roomCapacity}
            helperText={errors.roomCapacity?.message}
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

export default UpdateDormModal;
