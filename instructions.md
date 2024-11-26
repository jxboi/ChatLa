Project Overview

This is a minimalistic social media platform designed for friends and family to share posts easily. The application focuses on essential features to enable rapid prototyping and is designed mobile-first for optimal usability on smaller screens.

Technologies Used
- Frontend: React + TailwindCSS
- Backend: ASP.NET Core 8.0 Web API
- Database: PostgreSQL
- State Management: React Context API
- File Storage: Local storage or cloud storage service for images
- Version Control: Git

Project Setup Commands
---------------------

Initial Setup:

# IMPORTANT
do not create the root folder, just create the backend and frontend folders
run in bash terminal

# Create the project directory
mkdir backend
mkdir frontend

# Create the solution file
dotnet new sln -n chatlaapp

# Create the backend Web API project
dotnet new webapi -n chatlaapp.Backend -o backend

# Add the backend project to the solution
dotnet sln add backend/chatlaapp.Backend.csproj

# Create necessary directories in the backend
cd ./backend
mkdir Models Data Services

# Create the frontend React project using Vite
npm create vite@latest frontend -- --template react

# Add NuGet packages
cd ./backend
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Swashbuckle.AspNetCore

# Install frontend dependencies
cd ./frontend
npm install
npm install tailwindcss postcss autoprefixer
npm install @vitejs/plugin-react
npm install react-router-dom
npx tailwindcss init -p

Backend Setup

1. populate the Models
2. populate the Data
3. Add CreatePostDto and UpdatePostDto
4. populate the Filters including FileUploadOperationFilter


Folder Structure

chatlaapp/
├── backend/
│   ├── Controllers/
│   ├── Models/
│   ├── Data/
│   │   └── Migrations/
│   ├── Services/
│   ├── appsettings.json
│   ├── Program.cs
│   ├── Startup.cs
│   ├── chatlaapp.Backend.csproj
├── frontend/
│   ├── public/
│   │   ├── index.html
│   ├── src/
│   │   ├── components/
│   │   │   ├── common/
│   │   │   ├── layout/
│   │   │   └── features/
│   │   ├── pages/
│   │   ├── context/
│   │   ├── utils/
│   │   ├── App.js
│   │   └── index.js
│   ├── package.json
│   ├── tailwind.config.js
│   └── vite.config.js
├── chatlaapp.sln
├── README.md
├── .gitignore
└── LICENSE (optional)

Core Functionality

The platform prioritizes essential features to facilitate quick development of the initial prototype.

Dummy Login and Logout
- Login:
  - Users enter a username on a simple login page.
  - The username is stored locally on the client side and used as the user's identity.
  - No authentication or backend validation is included.

- Logout:
  - Users can log out, which clears the locally stored username.
  - After logging out, users are redirected back to the login page.

Post Management
- Create Posts:
  - Users can create posts with text content and an optional image upload.
  - Images are displayed in the feed alongside the text content.
  - Accepts image formats like JPEG and PNG.

- Edit Posts:
  - Users can edit their own posts, including replacing or removing the image.
  - Changes are reflected immediately.

- Delete Posts:
  - Users can delete their own posts, removing them from the feed.

Comments
- Add Comments:
  - Users can add comments to any post using their username.
  - Comments consist of text only.

- View Comments:
  - Comments are displayed below the corresponding post in chronological order.

- Delete Comments:
  - Users can delete their own comments.

Likes
- Like Posts:
  - Users can like any post in the feed.
  - A like is associated with the logged-in username.

- Unlike Posts:
  - Users can remove their like from a post they previously liked.

- Like Count:
  - Each post displays the total number of likes.

Search for Friends and Follow/Unfollow
- Search by Username:
  - Users can search for other users by entering their username in a search bar.

- Follow:
  - Users can follow other users from search results or their profile.
  - Posts from followed users appear in the feed.

- Unfollow:
  - Users can unfollow users they are no longer interested in.

- Following List:
  - Users can view a list of all the users they follow.

Feed
- View Posts:
  - Displays a chronological feed of posts from users that the logged-in user follows.
  - If no users are followed, the feed displays all posts.

- Infinite Scrolling:
  - Implements infinite scrolling, loading more posts as the user scrolls down.

- Post Content:
  - Posts include text content, images (if available), and metadata such as username and timestamp.

Data Models and Relationships

User
- Fields:
  - username (string)

Post
- Fields:
  - postId (UUID)
  - content (string)
  - imageUrl (string, nullable)
  - timestamp (datetime)
  - username (string)

- Relationships:
  - One-to-many relationship with Comment
  - One-to-many relationship with Like

Comment
- Fields:
  - commentId (UUID)
  - postId (UUID)
  - content (string)
  - timestamp (datetime)
  - username (string)

Like
- Fields:
  - likeId (UUID)
  - postId (UUID)
  - username (string)

Follow
- Fields:
  - followerUsername (string)
  - followingUsername (string)

API Endpoint Definitions

User Endpoints
- GET /api/users/search
  - Query Parameters:
    - username (string): The username to search for.
  - Description:
    - Returns a list of users matching the search query.

- POST /api/users/{username}/follow
  - Route Parameters:
    - username (string): The username of the user to follow.
  - Request Body:
    - currentUsername (string): The username of the logged-in user.
  - Description:
    - Adds a follow relationship between the logged-in user and the specified user.

- DELETE /api/users/{username}/follow
  - Route Parameters:
    - username (string): The username of the user to unfollow.
  - Request Body:
    - currentUsername (string): The username of the logged-in user.
  - Description:
    - Removes the follow relationship between the logged-in user and the specified user.

Post Endpoints
- GET /api/posts
  - Query Parameters:
    - currentUsername (string): Username of the logged-in user.
  - Description:
    - Retrieves posts from users that the logged-in user follows.
  - Response:
    - List of post objects with associated metadata.

- POST /api/posts
  - Request Body:
    - content (string): Text content of the post.
    - image (file, optional): Image file to upload.
    - username (string): Username of the poster.
  - Description:
    - Creates a new post with optional image upload.

- PUT /api/posts/{postId}
  - Route Parameters:
    - postId (UUID): ID of the post to edit.
  - Request Body:
    - content (string): Updated text content.
    - image (file, optional): New image file to upload or null to remove.
    - username (string): Username of the poster.
  - Description:
    - Updates an existing post if the username matches the poster.

- DELETE /api/posts/{postId}
  - Route Parameters:
    - postId (UUID): ID of the post to delete.
  - Request Body:
    - username (string): Username of the poster.
  - Description:
    - Deletes a post if the username matches the poster.

Comment Endpoints
- GET /api/posts/{postId}/comments
  - Route Parameters:
    - postId (UUID): ID of the post.
  - Description:
    - Retrieves all comments for the specified post.

- POST /api/posts/{postId}/comments
  - Route Parameters:
    - postId (UUID): ID of the post.
  - Request Body:
    - content (string): Text content of the comment.
    - username (string): Username of the commenter.
  - Description:
    - Adds a comment to the specified post.

- DELETE /api/comments/{commentId}
  - Route Parameters:
    - commentId (UUID): ID of the comment to delete.
  - Request Body:
    - username (string): Username of the commenter.
  - Description:
    - Deletes a comment if the username matches the commenter.

Like Endpoints
- POST /api/posts/{postId}/like
  - Route Parameters:
    - postId (UUID): ID of the post.
  - Request Body:
    - username (string): Username of the liker.
  - Description:
    - Adds a like to the specified post by the logged-in user.

- DELETE /api/posts/{postId}/like
  - Route Parameters:
    - postId (UUID): ID of the post.
  - Request Body:
    - username (string): Username of the liker.
  - Description:
    - Removes the like from the specified post by the logged-in user.

Search for Friends and Follow/Unfollow Endpoints
- GET /api/users/search
  - Query Parameters:
    - username (string): The username to search for.
  - Description:
    - Returns a list of users matching the search query.

- POST /api/users/{username}/follow
  - Route Parameters:
    - username (string): The username of the user to follow.
  - Request Body:
    - currentUsername (string): The username of the logged-in user.
  - Description:
    - Adds a follow relationship between the logged-in user and the specified user.

- DELETE /api/users/{username}/follow
  - Route Parameters:
    - username (string): The username of the user to unfollow.
  - Request Body:
    - currentUsername (string): The username of the logged-in user.
  - Description:
    - Removes the follow relationship between the logged-in user and the specified user.

Feed and Infinite Scrolling
- GET /api/posts
  - Query Parameters:
    - currentUsername (string): Username of the logged-in user.
  - Description:
    - Retrieves posts from users that the logged-in user follows.
  - Response:
    - List of post objects with associated metadata.

- Infinite Scrolling
  - Description:
    - Implements infinite scrolling by loading more posts dynamically as the user scrolls down.

Comment Endpoints
- GET /api/posts/{postId}/comments
  - Route Parameters:
    - postId (UUID): ID of the post.
  - Description:
    - Retrieves all comments for the specified post.

- POST /api/posts/{postId}/comments
  - Route Parameters:
    - postId (UUID): ID of the post.
  - Request Body:
    - content (string): Text content of the comment.
    - username (string): Username of the commenter.
  - Description:
    - Adds a comment to the specified post.

- DELETE /api/comments/{commentId}
  - Route Parameters:
    - commentId (UUID): ID of the comment to delete.
  - Request Body:
    - username (string): Username of the commenter.
  - Description:
    - Deletes a comment if the username matches the commenter.

Frontend State Management
- Global State:
  - Use React's Context API to manage global state, such as the logged-in user's information and follow lists.

- Local State:
  - Use local component state for UI-specific interactions and temporary data.

UI/UX Design Guidelines
- Mobile-First Design:
  - Optimize layouts for small screens first, then scale up for larger devices.
  - Use responsive design techniques to adjust to different screen sizes.

- Design Principles:
  - Clean and minimalistic interface with a neutral color palette.
  - Follow consistent design patterns for a cohesive user experience.
  - Use clear typography with legible font sizes.

Image Upload Handling
- Accepted Formats:
  - Support JPEG and PNG image formats.

- Storage Location:
  - Store images in `wwwroot/uploads/` or use a cloud storage service.

Search Functionality Behavior
- Case Sensitivity:
  - Implement case-insensitive search queries.

- Partial Matches:
  - Support 'starts with' logic for partial username matches.

Pagination and Data Loading
- Infinite Scrolling:
  - Implement infinite scrolling in the feed and search results.
  - Load additional content dynamically as the user scrolls down.

Additional Notes
- Future Enhancements:
  - The application structure allows for future addition of features like real authentication, direct messaging, and notifications.

- Scalability:
  - Design the codebase and database schema to allow for easy scaling, such as moving image storage to cloud services.

- Internationalization Preparedness:
  - Ensure the application supports Unicode characters in usernames, posts, and comments.

Project Setup Commands
---------------------

Initial Setup:

```bash
