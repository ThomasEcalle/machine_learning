cmake_minimum_required(VERSION 3.12)
project(library)

set(CMAKE_CXX_STANDARD 14)

set(EIGEN_DIR "/Users/thomasecalle/Development/machine_learning/library/eigen/Eigen")
include_directories(${EIGEN_DIR})

add_library(library SHARED library.cpp library.h source.cpp)
#add_executable(library library.cpp library.h source.cpp)
target_link_libraries(library ${EIGEN_DIR})