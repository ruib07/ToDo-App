import { ToastContainer } from "react-toastify";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import "react-toastify/dist/ReactToastify.css";

import Dashboard from "./pages/Dashboard";
import NotFound from "./pages/404";

import Signup from "./pages/Signup";
import Signin from "./pages/Signin";

import TaskCreation from "./components/Tasks/CreateTask";
import TaskDetails from "./components/Tasks/TaskDetails";

export default function App() {
    return (
        <Router>
            <div className="flex flex-col min-h-screen bg-gray-100">
                <ToastContainer />

                <div className="flex-grow container mx-auto">
                    <Routes>
                        <Route path="/" element={<Dashboard /> } />
                        <Route path="*" element={<NotFound />} />

                        <Route path="/Authentication/Signup" element={<Signup />} />
                        <Route path="/Authentication/Signin" element={<Signin />} />

                        <Route path="/Task/Create" element={<TaskCreation />} />
                        <Route path="/Task/:taskId" element={<TaskDetails />} />
                    </Routes>
                </div>
            </div>
        </Router>
    )
}