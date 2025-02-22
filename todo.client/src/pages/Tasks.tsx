import { useEffect, useState } from "react";
import Header from "../layouts/Header";
import { useNavigate } from "react-router-dom";
import { ITask, tasksStatusMap } from "../types/task";
import { GetTasksByUser, UpdateTaskStatus } from "../services/tasksService";
import RemoveTask from "../components/Modals/DeleteTaskModal";
import EditTaskModal from "../components/Modals/EditTaskModal";

export default function Tasks() {
    const [tasks, setTasks] = useState<ITask[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [editingTask, setEditingTask] = useState<ITask | null>(null);
    const [showDeleteModal, setShowDeleteModal] = useState<string | null>(null);
    const navigate = useNavigate();
    const userId = localStorage.getItem("userId") || sessionStorage.getItem("userId");

    useEffect(() => {
        const fetchUserTasks = async () => {
            if (!userId) {
                setLoading(false);
                return;
            }

            try {
                const userTasksResponse = await GetTasksByUser(userId);
                setTasks(userTasksResponse.data);
            } catch {
                setError("Failed to fetch tasks.");
            } finally {
                setLoading(false);
            }
        };

        fetchUserTasks();
    }, [userId]);

    const handleCompletedTask = async (taskId: string) => {
        try {
            await UpdateTaskStatus(taskId, 1);
            setTasks(prevTasks =>
                prevTasks.map(task =>
                    task.id === taskId ? { ...task, status: 1 } : task
                )
            );
        } catch {
            setError("Error completing task.");
        }
    };

    const handleUpdateTask = async () => {
        window.location.reload();
    };

    const handleDelete = () => {
        window.location.reload();
    };

    if (loading) {
        return (
            <p className="text-center text-gray-200">Loading your tasks...</p>
        );
    }

    if (error) {
        return <p className="text-center text-red-500">{error}</p>;
    }

    return (
        <>
            <Header />
            <div className="mt-[80px] p-8">
                <div className="flex justify-between items-center mb-6">
                    <h2 className="text-3xl font-bold text-gray-900">My Tasks</h2>
                    <button
                        onClick={() => navigate("/Task/Create")}
                        className="flex items-center bg-orange-500 text-white px-4 py-2 rounded-md hover:bg-orange-600 transition cursor-pointer"
                    >
                        <svg
                            xmlns="http://www.w3.org/2000/svg"
                            className="h-5 w-5 mr-2"
                            fill="none"
                            viewBox="0 0 24 24"
                            stroke="currentColor"
                        >
                            <path
                                strokeLinecap="round"
                                strokeLinejoin="round"
                                strokeWidth="2"
                                d="M12 4v16m8-8H4"
                            />
                        </svg>
                        Add Task
                    </button>
                </div>

                {tasks.length > 0 ? (
                    <div className="overflow-hidden rounded-t-lg border border-gray-300 shadow-lg">
                        <table className="min-w-full bg-white">
                            <thead className="bg-orange-500 text-white">
                                <tr>
                                    <th className="py-3 px-4 text-left">Task</th>
                                    <th className="py-3 px-4 text-left">Description</th>
                                    <th className="py-3 px-4 text-left">Due Date</th>
                                    <th className="py-3 px-4 text-left">Status</th>
                                    <th className="py-3 px-4 text-center">Actions</th>
                                </tr>
                            </thead>
                            <tbody>
                                {tasks.map((task, index) => (
                                    <tr
                                        key={task.id}
                                        className={`border-t border-gray-200 ${index % 2 === 0 ? "bg-gray-50" : "bg-white"
                                            } hover:bg-gray-100 transition-all`}
                                    >
                                        <td className="py-4 px-4 font-medium text-gray-900">
                                            <h3
                                                className="hover:text-blue-500 hover:underline cursor-pointer"
                                            >
                                                {task.title}
                                            </h3>
                                        </td>
                                        <td className="py-4 px-4 text-gray-600">{task.description}</td>
                                        <td className="py-4 px-4 text-gray-600">{new Date(task.dueDate).toLocaleString().slice(0, -3)}</td>
                                        <td className="py-4 px-4 text-gray-600">{tasksStatusMap[task.status]}</td>
                                        <td className="py-4 px-4 text-center">
                                            <button
                                                onClick={() => handleCompletedTask(task.id!)}
                                                className={`w-10 text-white p-2 rounded-md transition-all cursor-pointer ${task.status === 1
                                                        ? "bg-gray-400 cursor-not-allowed"
                                                        : "bg-green-500 hover:bg-green-600"
                                                    }`}
                                                disabled={task.status === 1}
                                            >
                                                ✓
                                            </button>
                                            <button
                                                onClick={() => setEditingTask(task)}
                                                className="ms-2 bg-blue-500 text-white p-2 rounded-md hover:bg-blue-600 transition-all cursor-pointer"
                                            >
                                                ✏️
                                            </button>
                                            {editingTask && (
                                                <EditTaskModal
                                                    task={editingTask}
                                                    onClose={() => setEditingTask(null)}
                                                    onConfirm={handleUpdateTask}
                                                />
                                            )}
                                            <button
                                                onClick={() => setShowDeleteModal(task.id!)}
                                                className="ms-2 bg-red-500 text-white p-2 rounded-md hover:bg-red-600 transition-all cursor-pointer"
                                            >
                                                🗑️
                                            </button>
                                            {showDeleteModal === task.id && (
                                                <RemoveTask
                                                    taskId={task.id!}
                                                    onClose={() => setShowDeleteModal(null)}
                                                    onConfirm={handleDelete}
                                                />
                                            )}
                                        </td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    </div>
                ) : (
                    <p className="text-center text-gray-600">
                        You have no tasks yet. Please add some tasks.
                    </p>
                )}
            </div>
        </>
    );
}
