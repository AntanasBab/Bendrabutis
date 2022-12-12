import React from "react";
import { Grid, Paper, Avatar, TextField, Button } from "@material-ui/core";
import LockOutlinedIcon from "@material-ui/icons/LockOutlined";
import * as yup from "yup";
import { useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import { RegisterModel } from "../../data/dataModels";
import axios from "axios";
import { UrlManager } from "../../utils/urlmanager";
import { useNavigate } from "react-router-dom";

const schema = yup
  .object({
    username: yup.string().required("Įveskite vartotojo vardą"),
    password: yup
      .string()
      .min(5, "Slaptažodis privalo būti bent 5 simbolių")
      .required("Privaloma"),
  })
  .required("Privaloma užpildyti");

const Register = () => {
  const navigate = useNavigate();
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm({
    resolver: yupResolver(schema),
  });

  const onSubmit = (data: any) => {
    const loginData: RegisterModel = data;
    axios
      .post(UrlManager.getRegisterEndpoint(), {
        username: loginData.username,
        password: loginData.password,
      })
      .then((response) => {
        navigate("/login");
      })
      .catch((err) => {
        console.log(err);
      });
  };

  const paperStyle = {
    padding: 20,
    height: "290px",
    width: 280,
    margin: "20px auto",
  };
  const avatarStyle = { backgroundColor: "#1bbd7e" };
  const btnstyle = { margin: "15px 0 15px 0" };

  return (
    <Grid>
      <Paper elevation={10} style={paperStyle}>
        <Grid container alignContent="center">
          <Avatar variant="square" style={avatarStyle}>
            <LockOutlinedIcon />
          </Avatar>
          <h2 className="font-medium mt-[5px]">
            Bendrabučio rezervacijos sistema
          </h2>
        </Grid>
        <form onSubmit={handleSubmit(onSubmit)}>
          <TextField
            label="Vartotojo vardas"
            placeholder="Įveskite vartotojo vardą"
            {...register("username")}
            error={!!errors.username}
            helperText={errors.username?.message}
            fullWidth
            required
          />
          <TextField
            label="Slaptažodis"
            placeholder="Įveskite slaptažodį"
            type="password"
            {...register("password")}
            error={!!errors.password}
            helperText={errors.password?.message}
            fullWidth
            required
          />
          <Button
            type="submit"
            color="primary"
            variant="contained"
            style={btnstyle}
            fullWidth
          >
            Registruotis
          </Button>
        </form>
      </Paper>
    </Grid>
  );
};

export default Register;
