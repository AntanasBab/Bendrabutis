import React from "react";
import * as yup from "yup";
import { useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import TextField from "@mui/material/TextField";
import Button from "@mui/material/Button";
import { UrlManager } from "../../utils/urlmanager";
import axios from "axios";
import Cookies from "universal-cookie";

export interface DormFormFields {
  name: string;
  address: string;
  roomCapacity: number;
}

const schema = yup
  .object({
    name: yup.string().required("Parasyk"),
    address: yup.string().required("Parasyk"),
    roomCapacity: yup.number().required("Parasyk"),
  })
  .required("UZPILDYK");

export const DormForm = () => {
  const cookies = new Cookies();
  const {
    handleSubmit,
    register,
    formState: { errors },
  } = useForm<DormFormFields>({
    resolver: yupResolver(schema),
  });

  const onSubmit = (data: DormFormFields) =>
    axios.post(
      UrlManager.getAllDormsEndpoint(),
      { ...data },
      {
        headers: {
          Authorization: `Bearer ${cookies.get("JWT")}`,
        },
      }
    );

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <TextField
        label="Bendrabučio pavadinimas"
        style={{ margin: "10px 0 0 0" }}
        placeholder="Įveskite bendrabučio pavadinimas"
        {...register("name")}
        error={!!errors.name}
        helperText={errors.name?.message}
        fullWidth
        required
      />
      <TextField
        label="Adresas"
        placeholder="Įveskite adresą"
        {...register("address")}
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
        error={!!errors.roomCapacity}
        helperText={errors.roomCapacity?.message}
        fullWidth
        required
      />
      <Button
        type="submit"
        color="primary"
        variant="contained"
        fullWidth
        style={{ margin: "15px 0px 0px 0px" }}
      >
        Sukurti
      </Button>
    </form>
  );
};
