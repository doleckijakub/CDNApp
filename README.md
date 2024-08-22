# About

This is a proof of concept [CDN](https://en.wikipedia.org/wiki/Content_delivery_network) app, allowing simple [uploading](#uploading-files) and [downloading](#downloading-files) of files.

Written in C# with .NET ASPNET for the backend and React for the frontend.

# Building

## Linux

Download dependencies

- `npm`
- `dotnet-runtime`
- `dotnet-sdk`
- `aspnet-runtime` (for Arch based distributions)
```console
$ sudo pacman -S npm dotnet-runtime dotnet-sdk aspnet-runtime
```

- `npm`
- `dotnet-runtime-7.0`
- `dotnet-sdk-7.0`
- `aspnetcore-runtime-7.0` (for Debian/Ubuntu derivatives)
```console
$ sudo apt install npm dotnet-runtime-7.0 dotnet-sdk-7.0 aspnetcore-runtime-7.0
```

Install npm dependencies

```console
$ cd frontend
$ npm install
$ cd ..
```

- Build the project

```console
$ make build frontend
```

# Running

## Linux

Either run `bin/Debug/net8.0/CDNApp` directly or run

```console
$ make run
```

# Uploading files

## Linux / MacOS

To upload a file called `file.txt` simply type the below command in your terminal

```console
$ curl https://cdn.doleckijakub.pl -T file.txt
```

The output of that command gives you a UUID and filename in JSON format like, allowing for later [download](#downloading-files). When piped to [jq](https://jqlang.github.io/jq) the output may look like this.

```json
{
  "uuid": "4be5f948-2caa-42e2-a932-32e79ce9c874",
  "filename": "Makefile"
}
```

## Windows

To upload a file called `file.txt` simply type the below command in PowerShell

```powershell
Invoke-WebRequest -Uri "https://cdn.doleckijakub.pl" -InFile "file.txt" -Method Put
```

The output of that command gives you a UUID and filename in JSON format like, allowing for later [download](#downloading-files).

# Downloading files

To download a file you need its [UUID and filename](#uploading-files). To download a file with specific UUID and filename either
- go to `https://cdn.doleckijakub.pl/upload/{UUID}` and click on the filename you want to download
or
- download it directly from `https://cdn.doleckijakub.pl/{YOUR_UUID}/{YOUR_FILENAME}`

# Public record of previous downloads

In this app, all the uploads are public by default. You can thus browse the recent uploads and find your or any other upload there.


# API Reference

### Upload a file
```http
PUT https://cdn.doleckijakub.pl/file.txt
Content-Type: text/plain

This is a sample text file content.
```

### Download a specific file
```http
GET https://cdn.doleckijakub.pl/{{uuid}}/file.txt
```

### Get the page showing the list of files for a specific upload by UUID
```http
GET https://cdn.doleckijakub.pl/upload/{{uuid}}
```

### Get all currently saved upload UUID's
```http
GET https://cdn.doleckijakub.pl/api/v1/all
```

### Get filelist of an upload by UUID
```http
GET https://cdn.doleckijakub.pl/api/v1/filesof/{{uuid}}
```