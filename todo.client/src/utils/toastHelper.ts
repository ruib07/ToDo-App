import { toast } from "react-toastify";

export const showToast = (message: string, type: "success" | "error") => {
    toast[type](message, {
        position: "bottom-right",
        autoClose: 5000,
        closeOnClick: true,
        draggable: true,
    });
};

export const showSuccessToast = () => showToast("Success", "success");
export const showErrorToast = () => showToast("Something went wrong", "error");