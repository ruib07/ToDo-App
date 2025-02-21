import { IUser } from "../types/user";
import apiRequest from "./helpers/apiService";

export const GetUserById = async (userId: string) => apiRequest("GET", `users/${userId}`, undefined, true);

export const UpdateUser = async (userId: string, newUserData: Partial<IUser>) => apiRequest("PUT", `users/${userId}`, newUserData, true);

export const DeleteUser = async (userId: string) => apiRequest("DELETE", `users/${userId}`, undefined, true);