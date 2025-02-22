import { useEffect, useState } from "react";
import Header from "../layouts/Header";
import { ITask, tasksStatusMap } from "../types/task";
import { GetTasksByUser } from "../services/tasksService";
import { GetUserById } from "../services/usersService";
import { useNavigate } from "react-router-dom";

export default function Dashboard() {
    const [user, setUser] = useState<{ name: string } | null>(null);
    const [tasks, setTasks] = useState<ITask[]>([]);
    const [, setError] = useState<string | null>(null);
    const [completedTasksCount, setCompletedTasksCount] = useState(0);
    const [totalTasksCount, setTotalTasksCount] = useState(0);
    const navigate = useNavigate();
    const userId = localStorage.getItem("userId") || sessionStorage.getItem("userId");

    useEffect(() => {
        if (!userId) return;

        const fetchUserAndTasks = async () => {
            try {
                const response = await GetUserById(userId);
                const { name } = response.data;
                setUser({ name });

                const tasksResponse = await GetTasksByUser(userId);
                const allTasks = tasksResponse.data;

                const completedTasks = allTasks.filter((task: ITask) => task.status === 1);
                setCompletedTasksCount(completedTasks.length);
                setTotalTasksCount(allTasks.length);

                const pendingTasks = allTasks.filter((task: ITask) => task.status === 0);
                setTasks(pendingTasks);
            } catch (error) {
                setError(`Failed to load user data: ${error}`);
            }
        };

        fetchUserAndTasks();
    }, [userId]);

    return (
        <>
            <Header />
            <br />
            <div className="overflow-hidden max-h-screen">
                <main className="pt-16 max-h-screen overflow-auto">
                    <div className="px-6 py-8">
                        <div className="max-w-4xl mx-auto">
                            <div className="bg-gray-100 rounded-3xl p-8 mb-5 shadow-md">
                                <h1 className="text-3xl font-bold text-gray-900 mb-10">
                                    Your Dashboard
                                </h1>
                                <hr className="my-10 border-gray-300" />

                                {!user ? (
                                    <div className="p-6 bg-red-200 rounded-xl text-gray-900 shadow-md text-center">
                                        <div className="font-semibold text-xl">
                                            Need to authenticate in order to see your stats
                                        </div>
                                    </div>
                                ) : (
                                    <div className="grid grid-cols-2 gap-x-10">
                                        <div>
                                            <h2 className="text-2xl font-semibold text-gray-800 mb-4">Stats</h2>
                                            <div className="grid grid-cols-2 gap-4">
                                                <div className="col-span-2">
                                                    <div className="p-6 bg-blue-200 rounded-xl shadow-md">
                                                        <div className="font-semibold text-xl text-gray-800 leading-none">
                                                            Good day, {user.name} 👋
                                                        </div>
                                                        <div className="mt-5">
                                                            <button
                                                                type="button"
                                                                className="inline-flex items-center justify-center cursor-pointer py-2 px-4 rounded-xl bg-white text-gray-800 hover:bg-blue-500 hover:text-white text-sm font-semibold transition"
                                                                onClick={() => navigate("/Tasks")}
                                                            >
                                                                Start tracking
                                                            </button>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div className="p-5 bg-green-200 rounded-xl text-gray-900 shadow-md">
                                                    <div className="font-bold text-md">✅ Tasks Finished</div>
                                                    <div className="mt-2 text-md font-semibold">{completedTasksCount}</div>
                                                </div>
                                                <div className="p-5 bg-indigo-200 rounded-xl text-gray-900 shadow-md">
                                                    <div className="font-bold text-md">📊 Task Progress</div>
                                                    <div className="mt-2 text-md font-semibold">
                                                        {completedTasksCount} of {totalTasksCount} completed
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div>
                                            <h2 className="text-2xl font-semibold text-gray-800 mb-4">
                                                📝 Tasks to complete
                                            </h2>
                                            <div className="space-y-4">
                                                {tasks.map((task, index) => (
                                                    <div
                                                        key={index}
                                                        onClick={() => navigate(`/Task/${task.id}`)}
                                                        className="p-5 bg-gray-100 rounded-xl cursor-pointer text-gray-800 shadow-md transition hover:shadow-lg hover:bg-gray-200"
                                                    >
                                                        <div className="flex justify-between text-gray-600 text-sm">
                                                            <span>Task #{index + 1}</span>
                                                            <span>
                                                                ⏳ Due: {new Date(task.dueDate).toLocaleString().slice(0, -3)}
                                                            </span>
                                                        </div>
                                                        <a
                                                            href="/"
                                                            className="font-semibold text-lg text-gray-900 hover:text-indigo-600 hover:underline"
                                                        >
                                                            {task.title}
                                                        </a>
                                                        <div className="mt-2 flex items-center text-gray-700">
                                                            <p className="text-sm font-medium">
                                                                ❗{tasksStatusMap[task.status]}
                                                            </p>
                                                        </div>
                                                    </div>
                                                ))}
                                            </div>
                                        </div>
                                    </div>
                                )}
                            </div>
                        </div>
                    </div>
                </main>
            </div>
        </>
    );
}
