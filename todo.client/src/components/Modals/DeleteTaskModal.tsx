import { DeleteTask } from "../../services/tasksService";
import { IDeleteTask } from "../../types/task";
import { showErrorToast } from "../../utils/toastHelper";

export default function RemoveTask({ taskId, onClose, onConfirm }: IDeleteTask) {
    const handleDelete = () => {
        try {
            DeleteTask(taskId);
            onConfirm();
            onClose();
        } catch {
            showErrorToast();
            onClose();
        }
    };

    return (
        <div className="fixed inset-0 flex items-center justify-center bg-gray-200 bg-opacity-50 backdrop-blur-sm z-50">
            <div className="bg-white rounded-lg p-6 w-full max-w-md shadow-xl border border-gray-300">
                <h2 className="text-lg font-semibold text-gray-900 text-center">
                    Are you sure you want to delete this task?
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

