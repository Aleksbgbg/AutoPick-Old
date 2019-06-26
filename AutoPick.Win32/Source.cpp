#include <Windows.h>

#define EXTERN_DECL extern "C" __declspec(dllexport)
#define CDECL __cdecl

struct BitmapCreation
{
	HWND sourceWindow;
	HDC sourceDeviceContext;
	HDC targetDeviceContext;
	HBITMAP bitmap;
};

HWND AutoPick_FindWindow(LPCSTR windowName)
{
	return FindWindow(nullptr, windowName);
}

EXTERN_DECL bool CDECL IsWindowOpen(LPCSTR windowName)
{
	return AutoPick_FindWindow(windowName) != nullptr;
}

EXTERN_DECL BitmapCreation CDECL ScreenshotWindowByName(LPCSTR windowName)
{
	HWND window = AutoPick_FindWindow(windowName);

	HDC sourceDeviceContext = GetDC(window);
	HDC targetDeviceContext = CreateCompatibleDC(sourceDeviceContext);

	RECT windowRect{ };
	GetWindowRect(window, &windowRect);

	HBITMAP bitmap = CreateCompatibleBitmap(
		sourceDeviceContext,
		windowRect.right - windowRect.left,
		windowRect.bottom - windowRect.top
	);
	SelectObject(targetDeviceContext, bitmap);

	PrintWindow(window, targetDeviceContext, PW_CLIENTONLY);

	BitmapCreation bitmapCreation;
	bitmapCreation.sourceWindow = window;
	bitmapCreation.sourceDeviceContext = sourceDeviceContext;
	bitmapCreation.targetDeviceContext = targetDeviceContext;
	bitmapCreation.bitmap = bitmap;

	return bitmapCreation;
}

EXTERN_DECL void CDECL CleanUpBitmapCreation(const BitmapCreation& bitmapCreation)
{
	ReleaseDC(bitmapCreation.sourceWindow, bitmapCreation.sourceDeviceContext);
	DeleteDC(bitmapCreation.targetDeviceContext);
	DeleteObject(bitmapCreation.bitmap);
}