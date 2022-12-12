import TextField from "@mui/material/TextField";
import React, { useEffect, useState } from "react";
import * as yup from "yup";
import { yupResolver } from "@hookform/resolvers/yup";
import { useForm } from "react-hook-form";
import Button from "@mui/material/Button";
import { CustomSelect } from "../../components/CustomSelect";
import { Dormitory } from "../../data/dataModels";
import { UrlManager } from "../../utils/urlmanager";
import axios from "axios";
import Stack from "@mui/material/Stack";
import Cookies from "universal-cookie";

interface FloorFormFields {
  number: number;
  dormitoryId: number;
}

const schema = yup.object({
  number: yup
    .number()
    .positive("Aukšto numeris privalo būti teigiamas")
    .integer("Aukšto numeris privalo būti sveikasis skaičius")
    .required("Aušto numeris yra privalomas laukas"),
});

export const FloorForm = () => {
  const cookies = new Cookies();
  const {
    handleSubmit,
    register,
    formState: { errors },
  } = useForm<FloorFormFields>({
    resolver: yupResolver(schema),
  });

  const [dormList, SetDormList] = useState<Dormitory[]>();

  useEffect(() => {
    axios
      .get<Dormitory[]>(UrlManager.getAllDormsEndpoint(), {
        headers: { Authorization: `Bearer ${cookies.get("JWT")}` },
      })
      .then((dorms) => {
        SetDormList(dorms.data);
      });
  }, []);

  const onSubmit = (data: any) => console.log(data);

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <Stack spacing={4}>
        <TextField
          label="Aukšto numeris"
          style={{ margin: "10px 0 0 0" }}
          placeholder="Įveskite aukšto numerį"
          {...register("number")}
          error={!!errors.number}
          helperText={errors.number?.message}
          fullWidth
          required
        />
        <CustomSelect
          label="Bendrabutis"
          placeholder="Pasirinkite bendrabutį"
          {...register("dormitoryId")}
          error={!!errors.dormitoryId}
          options={
            dormList?.map((dorm) => ({
              label: dorm.name,
              value: dorm.id,
            })) || []
          }
          fullWidth
          required
        />
        <Button type="submit" color="primary" variant="contained" fullWidth>
          Sukurti
        </Button>
      </Stack>
    </form>
  );
};
