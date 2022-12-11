import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import "./App.css";
import PrivateRoute from "./components/routes/privateRoute";
import { UserRoles } from "./data/dataModels";
import DormManagement from "./pages/dormitories/dormManagement";
import Home from "./pages/home/home";
import Login from "./pages/login/login";
import { Requests } from "./pages/requests/Requests";
import Rooms from "./pages/rooms/rooms";

function App() {
  return (
    <Router>
      <div>
        {/* A <Switch> looks through its children <Route>s and
          renders the first one that matches the current URL. */}
        <Routes>
          <Route path="/login" element={<Login />} />
          <Route path="/" element={<Home />} />
          <Route path="/freerooms" element={<Rooms />} />
          <Route
            path="/dormmanagement"
            element={
              <PrivateRoute path="/dormmanagement" role={UserRoles.Admin} />
            }
          />
          <Route path="/requestcreation" element={<Requests />} />
          <Route
            path="/owo"
            element={<PrivateRoute path="/owo" role={UserRoles.Admin} />}
          />
        </Routes>
      </div>
    </Router>
  );
}

export default App;
