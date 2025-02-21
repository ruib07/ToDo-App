import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import Header from "../../layouts/Header";
import { ITask, tasksStatusMap } from "../../types/task";
import { GetTasksById } from "../../services/tasksService";
import NotFound from "../../pages/404";

export default function TaskDetails() {
    const { taskId } = useParams<{ taskId: string }>();
    const [task, setTask] = useState<ITask | null>(null);
    const [, setError] = useState<string | null>(null);

    const userId = localStorage.getItem("userId") || sessionStorage.getItem("userId");

    useEffect(() => {
        const fetchTask = async () => {
            try {
                const res = await GetTasksById(taskId!);
                setTask(res.data);
            }
            catch {
                setError("Error fetching skill");
            }
        };

        fetchTask();
    }, [taskId, userId]);

    if (!task) {
        return (
            <NotFound />
        );
    }

    return (
        <>
            <Header /><br />
            <div className="mt-24 container mx-auto p-4 md:p-8">
                <div className="max-w-3xl mx-auto bg-gray-200 border border-gray-300 text-gray-800 rounded-xl shadow-lg overflow-hidden">
                    <div className="p-8">
                        <h2 className="text-3xl font-bold text-center mb-4">{task.title}</h2>
                        <div className="flex justify-center mb-6">
                            <div className="w-16 h-1 bg-orange-600 rounded-full"></div>
                        </div>

                        <div className="flex">
                            <p className="text-lg text-gray-500 mb-6">Description: </p>
                            <p className="ms-2 text-lg text-gray-700 mb-6">{task.description}</p>
                        </div>

                        <div className="flex">
                            <p className="text-lg text-gray-500 mb-6">Status: </p>
                            <p className="ms-2 text-lg text-gray-700 mb-6">{tasksStatusMap[task.status]}</p>
                        </div>

                        <div className="flex">
                            <p className="text-lg text-gray-500 mb-6">Due Date: </p>
                            <p className="ms-2 text-lg text-gray-700 mb-6">{new Date(task.dueDate).toLocaleString().slice(0, -3)}</p>
                        </div>
                    </div>
                </div>
            </div>
        </>
    )
}