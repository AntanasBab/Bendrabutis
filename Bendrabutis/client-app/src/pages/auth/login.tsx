import React from "react";
import {
  Grid,
  Paper,
  Avatar,
  TextField,
  Button,
  Typography,
  Link,
} from "@material-ui/core";
import LockOutlinedIcon from "@material-ui/icons/LockOutlined";
import * as yup from "yup";
import { useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import { LoginModel } from "../../data/dataModels";
import axios from "axios";
import { UrlManager } from "../../utils/urlmanager";
import Cookies from "universal-cookie";
import { useNavigate } from "react-router-dom";

const schema = yup
  .object({
    email: yup
      .string()
      .email("Privalo būti el. paštas")
      .required("Įveskite el. paštą"),
    password: yup.string().required(),
  })
  .required("Privaloma užpildyti");

const Login = () => {
  const cookies = new Cookies();
  const navigate = useNavigate();
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm({
    resolver: yupResolver(schema),
  });

  const onSubmit = (data: any) => {
    const loginData: LoginModel = data;
    axios
      .post(UrlManager.getLoginEndpoint(), {
        email: loginData.email,
        password: loginData.password,
      })
      .then((response) => {
        cookies.set("JWT", response.data.accessToken, { path: "/" });
        navigate("/");
      })
      .catch((err) => {
        console.log(err);
      });
  };

  const paperStyle = {
    padding: 20,
    height: "330px",
    width: 280,
    margin: "20px auto",
  };
  const avatarStyle = { backgroundColor: "#1bbd7e" };
  const btnstyle = { margin: "15px 0 15px 0" };
  const signupStyle = { margin: "0 0 0 5px" };

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
            label="El. paštas"
            placeholder="Įveskite el. paštą"
            {...register("email")}
            error={!!errors.email}
            helperText={errors.email?.message}
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
            Prisijungti
          </Button>
        </form>

        <Typography>
          {" "}
          Neturite paskyros ?
          <Link href="/register" style={signupStyle}>
            Registruotis
          </Link>
        </Typography>
      </Paper>
    </Grid>
  );
};

export default Login;
