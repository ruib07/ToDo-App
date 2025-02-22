import { ChangeEvent, useEffect, useState } from "react";
import { GetUserById, UpdateUser } from "../services/usersService";
import { IUser } from "../types/user";
import Header from "../layouts/Header";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faEye, faEyeSlash } from "@fortawesome/free-solid-svg-icons";
import DeleteAccount from "../components/Modals/DeleteAccountModal";

export default function Profile() {
    const [user, setUser] = useState<IUser | null>(null);
    const [isEditing, setIsEditing] = useState(false);
    const [visible, setVisible] = useState<boolean>(true);
    const [, setError] = useState<string | null>(null);
    const [showDeleteModal, setShowDeleteModal] = useState<string | null>(null);
    const [formData, setFormData] = useState<Partial<IUser>>({
        name: "",
        email: "",
        password: "",
    });
    const userId = localStorage.getItem("userId") || sessionStorage.getItem("userId");

    useEffect(() => {
        const fetchUser = async () => {
            try {
                const response = await GetUserById(userId!);
                setUser(response.data);
                setFormData({
                    name: response.data.name,
                    email: response.data.email,
                    password: ""
                });
            } catch {
                setError("Failed to load user information.");
            }
        };

        fetchUser();
    }, [userId]);

    const handleInputChange = (e: ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
        setFormData({ ...formData, [e.target.name]: e.target.value });
    };

    const handleEdit = () => setIsEditing(true);

    const handleCancel = () => {
        setFormData({
            name: user?.name || "",
            email: user?.email || "",
            password: ""
        });
        setIsEditing(false);
    };

    const handleSave = async () => {
        try {
            await UpdateUser(userId!, formData);
            setUser({ ...user!, ...formData });
            setIsEditing(false);
            window.location.reload();
        } catch {
            alert("Failed to update profile.");
        }
    };

    const togglePasswordVisibility = () => {
        setVisible(!visible);
    };

    return (
        <>
            <Header />
            <div className="flex items-start justify-center mt-[120px]">
                <div className="bg-gray-200 shadow-lg border border-gray-300 rounded-lg p-8 max-w-2xl w-full">
                    <h2 className="text-3xl font-bold text-gray-800 mb-6 text-center">
                        Profile
                    </h2>

                    {user ? (
                        <div className="flex items-center space-x-8">
                            <div className="flex-1">
                                <div className="grid grid-cols-2 gap-6 mb-6">
                                    <div>
                                        <input
                                            type="text"
                                            name="name"
                                            value={formData.name}
                                            onChange={handleInputChange}
                                            disabled={!isEditing}
                                            className={`w-full mt-1 p-2 border bg-gray-100 text-gray-900 ${isEditing
                                                ? "border-orange-500 focus:border-orange-600"
                                                : "border-gray-300"
                                                } rounded-md`}
                                        />
                                    </div>
                                    <div>
                                        <input
                                            type="email"
                                            name="email"
                                            value={formData.email}
                                            onChange={handleInputChange}
                                            disabled={!isEditing}
                                            className={`w-full mt-1 p-2 border bg-gray-100 text-gray-900 ${isEditing
                                                ? "border-orange-500 focus:border-orange-600"
                                                : "border-gray-300"
                                                } rounded-md`}
                                        />
                                    </div>
                                </div>

                                <div className="mb-6">
                                    <label className="block text-sm font-semibold text-gray-800">
                                        Password
                                    </label>
                                    <div className="relative">
                                        <input
                                            name="password"
                                            type={visible ? "password" : "text"}
                                            value={formData.password}
                                            onChange={handleInputChange}
                                            disabled={!isEditing}
                                            className={`w-full mt-1 p-2 pr-10 border bg-gray-100 text-gray-900 ${isEditing
                                                ? "border-orange-500 focus:border-orange-600"
                                                : "border-gray-300"
                                                } rounded-md`}
                                        />
                                        <span
                                            className="absolute inset-y-0 right-3 flex items-center cursor-pointer text-gray-800"
                                            onClick={togglePasswordVisibility}
                                        >
                                            <FontAwesomeIcon icon={visible ? faEye : faEyeSlash} />
                                        </span>
                                    </div>
                                </div>

                                {!isEditing ? (
                                    <button
                                        onClick={handleEdit}
                                        className="flex justify-center mt-4 w-full bg-blue-500 text-gray-200 py-2 rounded-md hover:bg-blue-600 transition cursor-pointer"
                                    >
                                        Edit Information ✏️
                                    </button>
                                ) : (
                                    <div className="mt-4 flex space-x-4">
                                        <button
                                            onClick={handleCancel}
                                            className="flex-1 bg-gray-300 text-gray-900 py-2 rounded-md hover:bg-gray-400 transition cursor-pointer"
                                        >
                                            Cancel
                                        </button>
                                        <button
                                            onClick={handleSave}
                                            className="flex-1 bg-blue-500 text-gray-200 py-2 rounded-md hover:bg-blue-600 transition cursor-pointer"
                                        >
                                            Save
                                        </button>
                                    </div>
                                )}
                                <button
                                    onClick={() => setShowDeleteModal(user.id!)}
                                    className="flex justify-center mt-4 w-full bg-red-500 text-gray-200 py-2 rounded-md hover:bg-red-600 transition cursor-pointer"
                                >
                                    Delete Account 🗑️
                                </button>
                                {showDeleteModal === user.id && (
                                    <DeleteAccount
                                        userId={user.id!}
                                        onClose={() => setShowDeleteModal(null)}
                                        onConfirm={() => {
                                            setUser(null);
                                            setShowDeleteModal(null);
                                        }}
                                    />

                                )}
                            </div>
                        </div>
                    ) : (
                        <p className="text-center text-gray-600">Loading profile...</p>
                    )}
                </div>
            </div>
        </>
    );
}