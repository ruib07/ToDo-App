export enum TasksStatus {
    Pending = 0,
    Completed = 1
}

export const tasksStatusMap: { [key: number]: string } = {
    [TasksStatus.Pending]: "Pending",
    [TasksStatus.Completed]: "Completed"
};

export interface ITask {
    id?: string;
    userId: string;
    title: string;
    description: string;
    status: number;
    dueDate: string;
}

export interface IDeleteTask {
    taskId: string;
    onClose: () => void;
    onConfirm: () => void;
}