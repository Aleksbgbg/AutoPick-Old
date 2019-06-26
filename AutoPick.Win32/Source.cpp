#define EXTERN_DECL extern "C" __declspec(dllexport)
#define CALL_CDECL __cdecl

#include <Windows.h>

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

EXTERN_DECL bool CALL_CDECL IsWindowOpen(LPCSTR windowName)
{
	return AutoPick_FindWindow(windowName) != nullptr;
}

EXTERN_DECL BitmapCreation CALL_CDECL ScreenshotWindowByName(LPCSTR windowName)
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

EXTERN_DECL void CALL_CDECL CleanUpBitmapCreation(const BitmapCreation& bitmapCreation)
{
	ReleaseDC(bitmapCreation.sourceWindow, bitmapCreation.sourceDeviceContext);
	DeleteDC(bitmapCreation.targetDeviceContext);
	DeleteObject(bitmapCreation.bitmap);
}