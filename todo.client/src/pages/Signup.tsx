import { FormEvent, useState } from "react";
import { useNavigate } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faEye, faEyeSlash } from "@fortawesome/free-solid-svg-icons";
import { IRegistration } from "../types/authentication";
import { Registration } from "../services/authenticationsService";
import { showErrorToast, showSuccessToast } from "../utils/toastHelper";
import Img from "/assets/ToDo-Logo.png";

export default function Signup() {
    const [name, setName] = useState("");
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [visible, setVisible] = useState(true);
    const navigate = useNavigate();

    const handleRegister = async (e: FormEvent) => {
        e.preventDefault();

        const newUser: IRegistration = {
            name,
            email,
            password
        };

        try {
            await Registration(newUser);
            showSuccessToast();
            navigate("/Authentication/Signin");
        } catch {
            showErrorToast();
        }
    };

    const togglePasswordVisibility = () => {
        setVisible(!visible);
    };

    return (
        <div className="flex min-h-full flex-1 justify-center px-6 py-12 lg:px-8">
            <div className="relative w-full max-w-lg">
                <img alt="SkillSwap" src={Img} className="mx-auto h-12 w-auto" /><br />

                <div className="w-full bg-gray-200 max-w-lg border border-gray-300 rounded-lg shadow">
                    <div className="p-6 space-y-4 md:space-y-6 sm:p-8">
                        <h1 className="text-xl font-bold leading-tight tracking-tight text-gray-800 md:text-2xl text-center">
                            Sign up
                        </h1>
                        <form className="space-y-4 md:space-y-6" onSubmit={handleRegister}>
                            <div>
                                <label className="block mb-2 text-sm font-medium text-gray-700">Full Name *</label>
                                <input
                                    type="text"
                                    className="bg-gray-100 border border-gray-300 text-gray-900 rounded-lg block w-full p-2.5"
                                    placeholder="Your Name"
                                    required
                                    value={name}
                                    onChange={(e) => setName(e.target.value)}
                                />
                            </div>

                            <div>
                                <label className="block mb-2 text-sm font-medium text-gray-700">Email *</label>
                                <input
                                    type="email"
                                    className="bg-gray-100 border border-gray-300 text-gray-900 rounded-lg block w-full p-2.5"
                                    placeholder="name@example.com"
                                    required
                                    value={email}
                                    onChange={(e) => setEmail(e.target.value)}
                                />
                            </div>

                            <div>
                                <label className="block mb-2 text-sm font-medium text-gray-700">Password *</label>
                                <div className="relative">
                                    <input
                                        type={visible ? "password" : "text"}
                                        className="bg-gray-100 border border-gray-300 text-gray-900 rounded-lg block w-full p-2.5 pr-10"
                                        placeholder="●●●●●●●●"
                                        required
                                        value={password}
                                        onChange={(e) => setPassword(e.target.value)}
                                    />
                                    <span
                                        className="absolute inset-y-0 right-0 flex items-center pr-3 cursor-pointer text-gray-800"
                                        onClick={togglePasswordVisibility}
                                    >
                                        <FontAwesomeIcon icon={visible ? faEye : faEyeSlash} />
                                    </span>
                                </div>
                            </div>

                            <button
                                type="submit"
                                className="w-full text-white bg-orange-500 hover:bg-orange-600 cursor-pointer font-medium rounded-lg text-sm px-5 py-2.5 text-center"
                            >
                                Sign Up
                            </button>

                            <p className="text-sm font-light text-gray-900 text-left">
                                Have an account?{" "}
                                <a
                                    href="/Authentication/Signin"
                                    className="font-medium text-orange-500 hover:underline">
                                    Sign In
                                </a>
                            </p>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    );
}