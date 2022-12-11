import * as React from "react";
import Footer from "../../components/footer/Footer";
import ResponsiveAppBar from "../../components/header/ResponsiveAppBar";
import { Container, Typography } from "@mui/material";

export default function Home() {
  return (
    <>
      <ResponsiveAppBar />
      <Container
        disableGutters
        maxWidth="sm"
        component="main"
        sx={{ pt: 8, pb: 6 }}
      >
        <Typography
          component="h1"
          variant="h2"
          align="center"
          color="text.primary"
          gutterBottom
        >
          Sveiki prisijungę prie KTU bendrabučių sistemos!
        </Typography>
        <Typography
          variant="h5"
          align="center"
          color="text.secondary"
          component="p"
        >
          Viską, kas susiję su gyvenimu bendrabutyje, rasite čia.
        </Typography>
      </Container>
      <Footer />
    </>
  );
}
