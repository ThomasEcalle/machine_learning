cmake_minimum_required(VERSION 3.12)
project(library)

set(CMAKE_CXX_STANDARD 14)

add_library(library SHARED library.cpp library.h source.cpp)
#add_executable(library library.cpp library.h source.cpp)