export interface IUser {
    id?: string;
    name: string;
    email: string;
    password: string;
}

export interface IDeleteUser {
    userId: string;
    onClose: () => void;
    onConfirm: () => void;
}