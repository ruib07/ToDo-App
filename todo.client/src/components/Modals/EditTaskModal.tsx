import { useState } from "react";
import { UpdateTask } from "../../services/tasksService";
import { ITask } from "../../types/task";
import { showErrorToast } from "../../utils/toastHelper";

export default function EditTaskModal({ task, onClose, onConfirm }: { task: ITask; onClose: () => void; onConfirm: () => void }) {
    const [editedTask, setEditedTask] = useState<Partial<ITask>>(task);

    const handleUpdate = async () => {
        try {
            await UpdateTask(task.id!, editedTask);
            onConfirm();
            onClose();
        } catch {
            showErrorToast();
        }
    };

    return (
        <div className="fixed inset-0 flex items-center justify-center bg-gray-200 bg-opacity-50 backdrop-blur-sm z-50">
            <div className="bg-white rounded-lg p-6 w-full max-w-md shadow-xl border border-gray-300">
                <h2 className="text-lg font-semibold text-gray-900 text-center">Edit Task</h2>
                <div className="mt-4">
                    <label className="block text-gray-700">Title</label>
                    <input
                        type="text"
                        value={editedTask.title || ""}
                        onChange={(e) => setEditedTask({ ...editedTask, title: e.target.value })}
                        className="w-full border border-gray-300 rounded-md p-2 mt-1"
                    />
                </div>
                <div className="mt-4">
                    <label className="block text-gray-700">Description</label>
                    <textarea
                        value={editedTask.description || ""}
                        onChange={(e) => setEditedTask({ ...editedTask, description: e.target.value })}
                        className="w-full border border-gray-300 rounded-md p-2 mt-1"
                    />
                </div>
                <div className="mt-4">
                    <label className="block text-gray-700">Due Date</label>
                    <input
                        type="datetime-local"
                        value={editedTask.dueDate || ""}
                        onChange={(e) => setEditedTask({ ...editedTask, dueDate: e.target.value })}
                        className="w-full border border-gray-300 rounded-md p-2 mt-1"
                    />
                </div>
                <div className="mt-6 flex justify-center space-x-4">
                    <button
                        className="bg-orange-500 text-white py-2 px-4 rounded-md hover:bg-orange-600 transition duration-200 cursor-pointer"
                        onClick={handleUpdate}
                    >
                        Save
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
