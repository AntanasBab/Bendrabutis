import React from "react";
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import "./App.css";
import { RolesAuthRoute } from "./components/routes/privateRoutes";
import { UserRoles } from "./data/dataModels";
import DormManagement from "./pages/dormitories/dormManagement";
import Home from "./pages/home/home";
import Login from "./pages/auth/login";
import { Requests } from "./pages/requests/Requests";
import Rooms from "./pages/rooms/rooms";
import Register from "./pages/auth/register";

function App() {
  return (
    <Router>
      <div>
        {/* A <Switch> looks through its children <Route>s and
          renders the first one that matches the current URL. */}
        <Routes>
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />
          <Route path="/" element={<Home />} />
          <Route path="/freerooms" element={<Rooms />} />
          <Route
            path="dormmanagement"
            element={
              <RolesAuthRoute role={UserRoles.Admin}>
                <DormManagement />
              </RolesAuthRoute>
            }
          />
          <Route path="/requestcreation" element={<Requests />} />
          <Route path="/profile" element={<></>} />
        </Routes>
      </div>
    </Router>
  );
}

export default App;
