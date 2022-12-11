import { ReactNode, Fragment } from "react";
import { Navigate } from "react-router-dom";
import { JWTAuthToken, UserRoles } from "../../data/dataModels";
import Cookies from "universal-cookie";
import jwt_decode from "jwt-decode";

export function RolesAuthRoute({
  children,
  role,
}: {
  children: ReactNode;
  role: UserRoles;
}) {
  const cookies = new Cookies();
  var userRoles = UserRoles.None;
  const token = cookies.get("JWT");

  if (token) {
    const role =
      jwt_decode<JWTAuthToken>(token)[
        "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
      ];
    console.log(role);
    switch (role) {
      case "Owner":
        userRoles = UserRoles.Owner;
        break;
      case "Admin":
        userRoles = UserRoles.Admin;
        break;
      case "Resident":
        userRoles = UserRoles.Resident;
        break;
      default:
        userRoles = UserRoles.None;
        break;
    }
  }

  const canAccess = role === userRoles;
  if (canAccess) return <Fragment>{children}</Fragment>;

  return <Navigate to="/login" replace={true} />;
}
