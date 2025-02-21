import { useEffect, useState } from "react";
import { Disclosure, DisclosureButton, DisclosurePanel, Menu, MenuButton, MenuItem, MenuItems } from "@headlessui/react";
import { Bars3Icon, XMarkIcon } from "@heroicons/react/24/outline";
import { useNavigate, useLocation } from "react-router-dom";
import { GetUserById } from "../services/usersService";
import { navigation } from "../data/headersNavigation";
import Icon from "/assets/ToDo-Logo.png";

function classNames(...classes: any) {
    return classes.filter(Boolean).join(" ");
}

export default function Header() {
    const [userData, setUserData] = useState<{ name: string; } | null>(null);
    const [, setError] = useState<string | null>(null);
    const navigate = useNavigate();
    const location = useLocation();
    const [isScrolled, setIsScrolled] = useState(false);

    useEffect(() => {
        const handleScroll = () => {
            setIsScrolled(window.scrollY > 50);
        };

        window.addEventListener("scroll", handleScroll);
        return () => window.removeEventListener("scroll", handleScroll);
    }, []);

    useEffect(() => {
        const fetchUserData = async () => {
            const token = localStorage.getItem("token") || sessionStorage.getItem("token");
            const userId = localStorage.getItem("userId") || sessionStorage.getItem("userId");

            if (!token || !userId) {
                setUserData(null);
                return;
            }

            try {
                const response = await GetUserById(userId!);
                const { name } = response.data;
                setUserData({ name });
            } catch (error) {
                setError(`Failed to load user data: ${error}`);
            }
        };

        fetchUserData();
    }, []);

    const handleSignOut = () => {
        localStorage.removeItem("token");
        sessionStorage.removeItem("token");
        localStorage.removeItem("userId");
        sessionStorage.removeItem("userId");

        setUserData(null);
        navigate("/");
        window.location.reload();
    };

    return (
        <Disclosure
            as="nav"
            className="fixed top-0 left-0 w-full z-20 transition duration-300"
        >
            <div
                className={`px-2 sm:px-6 lg:px-8 ${isScrolled ? "bg-gray-300 bg-opacity-90 shadow-md" : "bg-gray-200"} transition-all duration-300`}
            >
                <div className="relative flex h-16 items-center justify-between">
                    <div className="absolute inset-y-0 left-0 flex items-center sm:hidden">
                        <DisclosureButton className="group relative inline-flex items-center justify-center rounded-md p-2 text-gray-900 hover:text-orange-500 focus:outline-none focus:ring-2 focus:ring-inset focus:ring-white">
                            <Bars3Icon
                                aria-hidden="true"
                                className="block h-6 w-6 group-data-[open]:hidden"
                            />
                            <XMarkIcon
                                aria-hidden="true"
                                className="hidden h-6 w-6 group-data-[open]:block"
                            />
                        </DisclosureButton>
                    </div>
                    <div className="flex flex-1 items-center justify-center sm:items-stretch sm:justify-start">
                        <div className="flex flex-shrink-0 items-center">
                            <a href="/">
                                <img alt="ToDo" src={Icon} className="h-7 w-auto" />
                            </a>
                        </div>
                        <div className="hidden sm:ml-6 sm:block">
                            <div className="flex space-x-4">
                                {navigation.map((item) => (
                                    <a
                                        key={item.name}
                                        href={item.href}
                                        className={classNames(
                                            location.pathname === item.href
                                                ? "text-orange-500"
                                                : "text-gray-900 hover:text-orange-500",
                                            "rounded-md px-3 py-2 text-md font-medium"
                                        )}
                                    >
                                        {item.name}
                                    </a>
                                ))}
                            </div>
                        </div>
                    </div>
                    <div className="absolute inset-y-0 right-0 flex items-center pr-2 sm:static sm:inset-auto sm:ml-6 sm:pr-0">
                        {userData ? (
                            <Menu as="div" className="relative ml-3">
                                <div>
                                    <MenuButton className="flex items-center cursor-pointer text-md text-gray-900">
                                        <span className="ms-2 mr-2">Hi, {userData.name}</span>
                                    </MenuButton>
                                </div>
                                <MenuItems className="absolute right-0 z-10 mt-2 w-48 origin-top-right rounded-md bg-gray-200 py-1 shadow-lg ring-1 ring-gray-300 ring-opacity-5 focus:outline-none">
                                    <MenuItem>
                                        <a
                                            href="/MyInfo"
                                            className="block hover:bg-gray-300 px-4 py-2 text-md text-gray-800"
                                        >
                                            Your Profile
                                        </a>
                                    </MenuItem>
                                    <MenuItem>
                                        <button
                                            onClick={handleSignOut}
                                            className="block w-full hover:bg-gray-300 text-left px-4 py-2 text-md text-gray-800 cursor-pointer"
                                        >
                                            Sign out
                                        </button>
                                    </MenuItem>
                                </MenuItems>
                            </Menu>
                        ) : (
                            <Menu as="div" className="relative ml-3">
                                <MenuButton className="text-white text-sm cursor-pointer">
                                    Authentication
                                </MenuButton>
                                <MenuItems className="absolute right-0 z-10 mt-2 w-48 origin-top-right rounded-md bg-gray-800 py-1 shadow-lg ring-1 ring-gray-600 ring-opacity-5 focus:outline-none">
                                    <MenuItem>
                                        <a
                                            href="/Authentication/Login"
                                            className="block hover:bg-gray-300 px-4 py-2 text-md text-gray-800"
                                        >
                                            Login
                                        </a>
                                    </MenuItem>
                                    <MenuItem>
                                        <a
                                            href="/Authentication/Registration"
                                            className="block hover:bg-gray-300 px-4 py-2 text-md text-gray-800"
                                        >
                                            Registration
                                        </a>
                                    </MenuItem>
                                </MenuItems>
                            </Menu>
                        )}
                    </div>
                </div>
            </div>

            <DisclosurePanel className="bg-gray-200 border border-gray-300 sm:hidden">
                <div className="space-y-1 px-2 pb-3 pt-2">
                    {navigation.map((item) => (
                        <DisclosureButton
                            key={item.name}
                            as="a"
                            href={item.href}
                            className="block rounded-md px-3 py-2 text-base font-medium text-gray-900 hover:bg-gray-700 hover:text-white"
                        >
                            {item.name}
                        </DisclosureButton>
                    ))}
                </div>
            </DisclosurePanel>
        </Disclosure>
    );
}