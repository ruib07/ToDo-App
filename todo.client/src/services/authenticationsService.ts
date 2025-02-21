import { ILogin, IRegistration } from "../types/authentication";
import apiRequest from "./helpers/apiService";

export const Registration = async (newUser: IRegistration) => apiRequest("POST", "auth/signup", newUser, false);

export const Login = async (login: ILogin) => apiRequest("POST", "auth/signin", login, false);