import TextField from "@mui/material/TextField";
import React, { useState } from "react";
import * as yup from "yup";
import Button from "@mui/material/Button";
import { CustomSelect } from "../../components/CustomSelect";
import { UrlManager } from "../../utils/urlmanager";
import axios from "axios";
import Stack from "@mui/material/Stack";
import Cookies from "universal-cookie";

interface RequestFormFields {
  number: number;
  dormitoryId: number;
}

const valList = [
  { value: 1, label: "Įsikelti" },
  { value: 2, label: "Išsikelti" },
  { value: 3, label: "Pakeisti kambarį" },
];

export const RequestForm = () => {
  const [reqType, setReqType] = useState<number>();
  const [description, setDescription] = useState<string>();

  const cookies = new Cookies();

  const onSubmit = () => {
    axios
      .post(
        UrlManager.getCreateRequestEndpoint(),
        { type: reqType, description: description },
        {
          headers: {
            Authorization: `Bearer ${cookies.get("JWT")}`,
          },
        }
      )
      .then(() => window.location.reload());
  };

  return (
    <form>
      <Stack spacing={4} sx={{ width: "75%" }}>
        <CustomSelect
          label="Prašymo tipas"
          placeholder="Pasirinkite prašymo tipą"
          options={
            valList?.map((dorm) => ({
              label: dorm.label,
              value: dorm.value,
            })) || []
          }
          onChange={(e) => setReqType(Number(e.target.value))}
          fullWidth
          required
        />
        <TextField
          label="Aprašymas"
          style={{ margin: "10px 0 0 0" }}
          placeholder="Įveskite prašymo tekstą"
          onChange={(e) => setDescription(e.target.value)}
          fullWidth
          required
        />

        <Button
          type="submit"
          color="primary"
          variant="contained"
          fullWidth
          onClick={() => onSubmit()}
        >
          Sukurti
        </Button>
      </Stack>
    </form>
  );
};
