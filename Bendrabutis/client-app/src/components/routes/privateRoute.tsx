// This is used to determine if a user is authenticated and
// if they are allowed to visit the page they navigated to.

// If they are: they proceed to the page
// If not: they are redirected to the login page.

import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { UserRoles } from "../../data/dataModels";
import Cookies from "universal-cookie";

interface PrivateRouteProps {
  path: string;
  role: UserRoles;
}

const PrivateRoute = (props: PrivateRouteProps) => {
  const navigate = useNavigate();
  const cookies = new Cookies();

  useEffect(() => {
    const token = cookies.get("JWT");
    if (!token) {
      navigate("/login");
    }

    let role: UserRoles = UserRoles.None;
    switch (cookies.get("Role")) {
      case "Owner":
        role = UserRoles.Owner;
        break;
      case "Admin":
        role = UserRoles.Admin;
        break;
      case "Resident":
        role = UserRoles.Resident;
        break;
      default:
        role = UserRoles.None;
        navigate("/login");
        break;
    }
    if (role === props.role) {
      navigate(props.path);
    }
  }, []);

  return <></>;
};

export default PrivateRoute;
