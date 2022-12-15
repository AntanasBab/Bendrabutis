import TextField from "@mui/material/TextField";
import React, { useEffect, useState } from "react";
import * as yup from "yup";
import { yupResolver } from "@hookform/resolvers/yup";
import { useForm } from "react-hook-form";
import Button from "@mui/material/Button";
import { CustomSelect } from "../../components/CustomSelect";
import { DormFloor } from "../../data/dataModels";
import { UrlManager } from "../../utils/urlmanager";
import axios from "axios";
import Stack from "@mui/material/Stack";
import Cookies from "universal-cookie";

interface RoomFormFields {
  number: number;
  numberOfLivingPlaces: number;
  area: number;
  floorId: number;
}

const schema = yup.object({
  number: yup
    .number()
    .positive("Kambario numeris privalo būti teigiamas")
    .integer("Kambario numeris privalo būti sveikasis skaičius")
    .required("Kambario numeris yra privalomas laukas"),
  numberOfLivingPlaces: yup
    .number()
    .positive("Kambario numeris privalo būti teigiamas")
    .integer("Kambario numeris privalo būti sveikasis skaičius")
    .required("Kambario numeris yra privalomas laukas"),
  area: yup
    .number()
    .positive("Kambario numeris privalo būti teigiamas")
    .required("Kambario numeris yra privalomas laukas"),
});

export const RoomForm = () => {
  const cookies = new Cookies();
  const {
    handleSubmit,
    register,
    formState: { errors },
  } = useForm<RoomFormFields>({
    resolver: yupResolver(schema),
  });

  const [floorList, setFloorList] = useState<DormFloor[]>();

  useEffect(() => {
    axios
      .get<DormFloor[]>(UrlManager.getAllDormFloorsEndpoint(), {
        headers: { Authorization: `Bearer ${cookies.get("JWT")}` },
      })
      .then((floors) => {
        setFloorList(floors.data);
      });
  }, []);

  const onSubmit = (data: any) =>
    axios
      .post(
        UrlManager.getCreateRoomEndpoint(
          data.floorId,
          data.number,
          data.numberOfLivingPlaces,
          data.area
        ),
        {},
        {
          headers: {
            Authorization: `Bearer ${cookies.get("JWT")}`,
          },
        }
      )
      .then(() => window.location.reload());

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <Stack spacing={4} sx={{ width: "75%" }}>
        <TextField
          label="Kambario numeris"
          style={{ margin: "10px 0 0 0" }}
          placeholder="Įveskite kambario numerį"
          {...register("number")}
          error={!!errors.number}
          helperText={errors.number?.message}
          fullWidth
          required
        />
        <TextField
          label="Gyvenamų vietų skaičius"
          style={{ margin: "10px 0 0 0" }}
          placeholder="Įveskite gyvenamų vietų skaičių"
          {...register("numberOfLivingPlaces")}
          error={!!errors.numberOfLivingPlaces}
          helperText={errors.numberOfLivingPlaces?.message}
          fullWidth
          required
        />
        <TextField
          label="Kambario plotas"
          style={{ margin: "10px 0 0 0" }}
          placeholder="Įveskite kambario plotą"
          {...register("area")}
          error={!!errors.area}
          helperText={errors.area?.message}
          fullWidth
          required
        />
        <CustomSelect
          label="Aukštas"
          placeholder="Pasirinkite aukštą"
          {...register("floorId")}
          error={!!errors.floorId}
          options={
            floorList?.map((floor) => ({
              label: `Aukštas nr. ${floor.number}`,
              value: floor.id,
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
