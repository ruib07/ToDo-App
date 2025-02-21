import { ITask, TasksStatus } from "../types/task";
import apiRequest from "./helpers/apiService";

export const GetTasksByUser = async (userId: string) => apiRequest("GET", `tasks/byuser/${userId}`, undefined, true);

export const GetTasksById = async (taskId: string) => apiRequest("GET", `tasks/${taskId}`, undefined, true);

export const CreateTask = async (newTask: ITask) => apiRequest("POST", "tasks", newTask, true);

export const UpdateTask = async (taskId: string, newTaskData: Partial<ITask>) => apiRequest("PUT", `tasks/${taskId}`, newTaskData, true);

export const UpdateTaskStatus = async (taskId: string, status: TasksStatus) => apiRequest("PATCH", `tasks/${taskId}/status`, status, true);

export const DeleteTask = async (taskId: string) => apiRequest("DELETE", `tasks/${taskId}`, undefined, true);