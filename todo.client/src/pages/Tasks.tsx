import { useEffect, useState } from "react";
import Header from "../layouts/Header";
import { useNavigate } from "react-router-dom";
import { ITask, tasksStatusMap } from "../types/task";
import { GetTasksByUser } from "../services/tasksService";
import RemoveTask from "../components/Modals/DeleteTaskModal";

export default function Tasks() {
    const [tasks, setTasks] = useState<ITask[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
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
                                                onClick={() => setShowDeleteModal(task.id!)}
                                                className="bg-red-500 text-white p-2 rounded-md hover:bg-red-600 transition-all cursor-pointer"
                                            >
                                                <svg
                                                    xmlns="http://www.w3.org/2000/svg"
                                                    fill="none"
                                                    viewBox="0 0 24 24"
                                                    strokeWidth="1.5"
                                                    stroke="currentColor"
                                                    className="w-5 h-5"
                                                >
                                                    <path
                                                        strokeLinecap="round"
                                                        strokeLinejoin="round"
                                                        d="m14.74 9-.346 9m-4.788 0L9.26 9m9.968-3.21c.342.052.682.107 1.022.166m-1.022-.165L18.16 19.673a2.25 2.25 0 0 1-2.244 2.077H8.084a2.25 2.25 0 0 1-2.244-2.077L4.772 5.79m14.456 0a48.108 48.108 0 0 0-3.478-.397m-12 .562c.34-.059.68-.114 1.022-.165m0 0a48.11 48.11 0 0 1 3.478-.397m7.5 0v-.916c0-1.18-.91-2.164-2.09-2.201a51.964 51.964 0 0 0-3.32 0c-1.18.037-2.09 1.022-2.09 2.201v.916m7.5 0a48.667 48.667 0 0 0-7.5 0"
                                                    />
                                                </svg>
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
