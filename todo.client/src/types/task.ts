export enum TasksStatus {
    Pending = 0,
    InProgress = 1,
    Completed = 2
}

export const tasksStatusMap: { [key: number]: string } = {
    [TasksStatus.Pending]: "Pending",
    [TasksStatus.InProgress]: "InProgress",
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