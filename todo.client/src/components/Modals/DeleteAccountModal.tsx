import { IDeleteUser } from "../../types/user";
import { DeleteUser } from "../../services/usersService";
import { showErrorToast } from "../../utils/toastHelper";
import { useNavigate } from "react-router-dom";

export default function DeleteAccount({ userId, onClose, onConfirm }: IDeleteUser) {
    const navigate = useNavigate();

    const handleDelete = async () => {
        try {
            await DeleteUser(userId);
            localStorage.removeItem("token");
            sessionStorage.removeItem("token");
            localStorage.removeItem("userId");
            sessionStorage.removeItem("userId");

            onConfirm();
            onClose();
            navigate("/");
            window.location.reload();
        } catch {
            showErrorToast();
            onClose();
        }
    };

    return (
        <div className="fixed inset-0 flex items-center justify-center bg-gray-200 bg-opacity-50 backdrop-blur-sm z-50">
            <div className="bg-white rounded-lg p-6 w-full max-w-md shadow-xl border border-gray-300">
                <h2 className="text-lg font-semibold text-gray-900 text-center">
                    Are you sure that you want to delete your account?
                </h2>
                <p className="text-gray-600 text-center mt-2">
                    This action cannot be undone.
                </p>
                <div className="flex justify-center space-x-4 mt-6">
                    <button
                        className="bg-red-500 text-white py-2 px-4 rounded-md hover:bg-red-600 transition duration-200 cursor-pointer"
                        onClick={handleDelete}
                    >
                        Yes, Delete
                    </button>
                    <button
                        className="bg-gray-300 text-gray-900 py-2 px-4 rounded-md hover:bg-gray-400 transition duration-200 cursor-pointer"
                        onClick={onClose}
                    >
                        Cancel
                    </button>
                </div>
            </div>
        </div>
    );
}