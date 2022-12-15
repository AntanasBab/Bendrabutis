import {
  Typography,
  Grid,
  Card,
  CardActionArea,
  CardContent,
  Stack,
  Button,
} from "@mui/material";
import { JWTAuthToken, UserRequest } from "../../data/dataModels";
import Cookies from "universal-cookie";
import axios from "axios";
import React, { useState, useEffect } from "react";
import Footer from "../../components/footer/Footer";
import ResponsiveAppBar from "../../components/header/ResponsiveAppBar";
import EditIcon from "@mui/icons-material/Edit";
import { UrlManager } from "../../utils/urlmanager";
import UpdateRequestModal from "../../components/modal/request/updateRequestModal";
import { RequestForm } from "./RequestForm";
import jwt_decode from "jwt-decode";
import DeleteIcon from "@mui/icons-material/Delete";
import DeleteRequestModal from "../../components/modal/request/deleteRequestModal";

const RequestManagement = (): JSX.Element => {
  const cookies = new Cookies();
  const token = cookies.get("JWT");
  const [userRequests, setUserRequests] = useState<UserRequest[]>();
  const [updateModalOpen, setUpdateModalOpen] = useState(false);
  const [deleteModalOpen, setDeleteModalOpen] = React.useState(false);
  const [selectedRequest, setSelectedRequest] = useState<UserRequest>();
  const showReq =
    token &&
    jwt_decode<JWTAuthToken>(token)[
      "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
    ] === "Admin";

  useEffect(() => {
    axios
      .get<UserRequest[]>(UrlManager.geAllRequestsEndpoint(), {
        headers: { Authorization: `Bearer ${token}` },
      })
      .then((userRequests) => {
        console.log(userRequests);
        setUserRequests(userRequests.data);
      });
  }, []);

  return (
    <>
      <ResponsiveAppBar />
      <Grid
        container
        spacing={2}
        justifyContent="flex-start"
        alignItems="center"
        sx={{ m: 2, width: "90%" }}
      >
        {userRequests?.length === 0 ? (
          <Typography gutterBottom variant="h5" component="div">
            Nėra prašymų
          </Typography>
        ) : (
          <Grid item md={6}>
            <Grid item xs={"auto"}>
              <Typography gutterBottom variant="h4" component="div">
                Mano prašymai
              </Typography>
            </Grid>
            <Grid
              container
              direction="row"
              justifyContent="flex-start"
              alignItems="flex-start"
              spacing={1}
            >
              {userRequests?.map((userRequest, index) => (
                <Grid item key={index} xs={"auto"} className="mt-1">
                  <Card sx={{ maxWidth: 345 }}>
                    <CardActionArea>
                      <CardContent>
                        <Typography gutterBottom variant="h4" component="div">
                          {userRequest.author.userName}
                        </Typography>
                        <Typography variant="h6" color="text.secondary">
                          {userRequest.author.email}
                        </Typography>
                        <Typography variant="body1" color="text.secondary">
                          {userRequest.description}
                        </Typography>
                        <Stack
                          direction="row"
                          spacing={2}
                          className={"flex mt-6"}
                        >
                          {showReq && (
                            <Button
                              variant="outlined"
                              startIcon={<DeleteIcon />}
                              onClick={() => {
                                setSelectedRequest(userRequest);
                                setDeleteModalOpen(true);
                              }}
                            >
                              Panaikinti
                            </Button>
                          )}
                          <Button
                            variant="contained"
                            endIcon={<EditIcon />}
                            onClick={() => {
                              setSelectedRequest(userRequest);
                              setUpdateModalOpen(true);
                            }}
                          >
                            Redaguoti
                          </Button>
                        </Stack>
                      </CardContent>
                    </CardActionArea>
                  </Card>
                </Grid>
              ))}
            </Grid>
          </Grid>
        )}
        <Grid
          item
          md={6}
          direction="row"
          justifyContent="flex-start"
          alignItems="flex-start"
        >
          <Typography gutterBottom variant="h5" component="div">
            Naujas prašymas:
          </Typography>
          <RequestForm />
        </Grid>
      </Grid>

      <DeleteRequestModal
        open={deleteModalOpen}
        request={selectedRequest}
        onClose={() => setDeleteModalOpen(false)}
      />
      <UpdateRequestModal
        open={updateModalOpen}
        request={selectedRequest}
        onClose={() => setUpdateModalOpen(false)}
      />
      <Footer />
    </>
  );
};

export default RequestManagement;
