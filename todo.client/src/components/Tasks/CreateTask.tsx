import { FormEvent, useState } from "react";
import { useNavigate } from "react-router-dom";
import { ITask } from "../../types/task";
import { CreateTask } from "../../services/tasksService";
import { showErrorToast, showSuccessToast } from "../../utils/toastHelper";
import Img from "/assets/ToDo-Logo.png";

export default function TaskCreation() {
    const [title, setTitle] = useState("");
    const [description, setDescription] = useState("");
    const [dueDate, setDueDate] = useState("");
    const navigate = useNavigate();
    const userId = localStorage.getItem("userId") || sessionStorage.getItem("userId");

    const handleSkillCreation = async (e: FormEvent) => {
        e.preventDefault();

        const newTask: ITask = {
            userId: userId!,
            title,
            description,
            status: 0,
            dueDate
        };

        try {
            await CreateTask(newTask);
            showSuccessToast();
            navigate("/Tasks");
        } catch {
            showErrorToast();
        }
    };

    return (
        <div className="flex min-h-full flex-1 justify-center px-6 py-12 lg:px-8">
            <div className="relative w-full max-w-lg">
                <img alt="ToDo" src={Img} className="mx-auto h-12 w-auto" /><br />

                <div className="w-full bg-gray-200 max-w-lg border border-gray-300 rounded-lg shadow">
                    <div className="p-6 space-y-4 md:space-y-6 sm:p-8">
                        <h1 className="text-xl font-bold leading-tight tracking-tight text-gray-800 md:text-2xl text-center">
                            Create Skill
                        </h1>
                        <form className="space-y-4 md:space-y-6" onSubmit={handleSkillCreation}>
                            <div>
                                <label className="block mb-2 text-sm font-medium text-gray-700">Title *</label>
                                <input
                                    type="text"
                                    className="bg-gray-100 border border-gray-300 text-gray-900 rounded-lg block w-full p-2.5"
                                    placeholder="Task title"
                                    required
                                    value={title}
                                    onChange={(e) => setTitle(e.target.value)}
                                />
                            </div>

                            <div>
                                <label className="block mb-2 text-sm font-medium text-gray-700">Description *</label>
                                <textarea
                                    className="bg-gray-100 border border-gray-300 text-gray-900 rounded-lg block w-full p-2.5"
                                    rows={4}
                                    placeholder="Tell more about this task..."
                                    value={description}
                                    onChange={(e) => setDescription(e.target.value)}
                                />
                            </div>

                            <div>
                                <label className="block mb-2 text-sm font-medium text-gray-700">Due Date *</label>
                                <input
                                    type="datetime-local"
                                    className="bg-gray-100 border border-gray-300 text-gray-900 rounded-lg block w-full p-2.5"
                                    required
                                    value={dueDate}
                                    onChange={(e) => setDueDate(e.target.value)}
                                />
                            </div>

                            <button
                                type="submit"
                                className="w-full text-white bg-orange-500 hover:bg-orange-600 cursor-pointer font-medium rounded-lg text-sm px-5 py-2.5 text-center"
                            >
                                Create Task
                            </button>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    );
}