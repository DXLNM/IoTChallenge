using IoTChallenge.Universal.Core;
using IoTChallenge.Universal.Core.Classes;
using Microsoft.ProjectOxford.Face;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace IoTChallenge.RPiMotionCamera
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private MediaCapture mediaCapture;
        private StorageFile photoFile;
        private string PHOTO_FILE_NAME;
        private bool isPreviewing;
        private bool isRecording;
        BitmapImage bitmap;
        private string photoFilePath;
        private readonly IFaceServiceClient faceServiceClient = new FaceServiceClient("628db5c61a9644f293a2015159ce8d53");
        private string gender;
        private double age;
        //private bool _ledStatus = false;
        private const int LED_MOTION = 6; // Used to detect motion
        private GpioPin _pinMotion;

        public MainPage()
        {
            this.InitializeComponent();
            isPreviewing = false;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            initializeCamera();
            //InitGPIO();   //Uncomment this line if you are debugging/deploying on the RPi2 
        }

        private void InitGPIO()
        {
            var gpio = GpioController.GetDefault();
            _pinMotion = gpio.OpenPin(LED_MOTION);
            _pinMotion.SetDriveMode(GpioPinDriveMode.Input);
            _pinMotion.ValueChanged += _pinMotion_ValueChanged;
        }

        private void _pinMotion_ValueChanged(GpioPin sender,
           GpioPinValueChangedEventArgs args)
        {
            var isOn = args.Edge == GpioPinEdge.FallingEdge;
            if (isOn)
            {
                //takePicture and call oxford service;
                Debug.WriteLine("Movement Detected");

            }


        }

        private void MainPage_Unloaded(object sender, object args)
        {
            _pinMotion.Dispose();
        }

        private async void initializeCamera()
        {
            try
            {
                if (mediaCapture != null)
                {
                    // Cleanup MediaCapture object
                    if (isPreviewing)
                    {
                        await mediaCapture.StopPreviewAsync();
                        isPreviewing = false;
                    }
                    if (isRecording)
                    {
                        await mediaCapture.StopRecordAsync();
                        isRecording = false;
                    }
                    mediaCapture.Dispose();
                    mediaCapture = null;
                }

                status.Text = "Initializing camera to capture audio and video...";
                // Use default initialization
                mediaCapture = new MediaCapture();
                await mediaCapture.InitializeAsync();

                // Set callbacks for failure and recording limit exceeded
                status.Text = "Device successfully initialized for video recording!";
                mediaCapture.Failed += new MediaCaptureFailedEventHandler(mediaCapture_Failed);
                //mediaCapture.RecordLimitationExceeded += new Windows.Media.Capture.RecordLimitationExceededEventHandler(mediaCapture_RecordLimitExceeded);

                // Start Preview                
                previewElement.Source = mediaCapture;
                await mediaCapture.StartPreviewAsync();
                isPreviewing = true;
                status.Text = "Camera preview succeeded";
            }
            catch (Exception ex)
            {
                status.Text = "Unable to initialize camera for audio/video mode: " + ex.Message;
            }
        }

        private void mediaCapture_Failed(MediaCapture currentCaptureObject, MediaCaptureFailedEventArgs currentFailure)
        {
            status.Text = currentFailure.Message;
        }

        private void resultBtn_Click(object sender, RoutedEventArgs e)
        {
            capturePicture();

            status.Text = "Detecting...";

            //FaceRectangle[] faceRects = await UploadAndDetectFaces(photoFilePath);

            //DetectedResultsInText = string.Format("Detecting...");

            status.Text += string.Format("Request: Detecting {0}", photoFilePath);

        }

        private async void capturePicture()
        {
            try
            {
                PHOTO_FILE_NAME = "LNM_Demo_" + (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds + ".jpg";
                captureImage.Source = null;

                photoFile = await KnownFolders.PicturesLibrary.CreateFileAsync(
                    PHOTO_FILE_NAME, CreationCollisionOption.ReplaceExisting);
                ImageEncodingProperties imageProperties = ImageEncodingProperties.CreateJpeg();
                await mediaCapture.CapturePhotoToStorageFileAsync(imageProperties, photoFile);
                status.Text = "Take Photo succeeded: " + photoFile.Path;

                IRandomAccessStream photoStream = await photoFile.OpenReadAsync();
                bitmap = new BitmapImage();
                bitmap.SetSource(photoStream);
                photoFilePath = photoFile.Path;
                captureImage.Source = bitmap;

                await Task.Run(() => identifyFace());
            }
            catch (Exception ex)
            {
                status.Text = ex.Message;
            }
        }

        private async Task identifyFace()
        {
            if (App.ConnectedToInternet())
            {
                // Call detection REST API
                using (Stream fileStream = File.OpenRead(photoFilePath))
                {
                    try
                    {
                        var faces = await faceServiceClient.DetectAsync(fileStream, analyzesAge: true, analyzesGender: true, analyzesHeadPose: true);

                        foreach (var face in faces)
                        {
                            var attributes = face.Attributes;

                            age = attributes.Age;

                            gender = attributes.Gender.ToString();

                            Debug.WriteLine("Age = " + age.ToString());
                            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                            () =>
                            {
                                resultTB.Text = "Age = " + age.ToString() + "  Gender = " + gender.ToString();
                            });
                        }

                        CameraSensorDocument document = await documentDBUtilities.getCameraDocument("bBNBAPFVBgAMAAAAAAAAAA==");
                        foreach (var item in document.id)
                        {
                            Debug.WriteLine(document.id);
                            Debug.WriteLine(document.description);
                            Debug.WriteLine(document.kiosk);
                            Debug.WriteLine(document.product);
                            Debug.WriteLine(document.unitPrice);
                            Debug.WriteLine(document.visits[0].ageOfPerson);
                        }
                        //document.visits.Add(new Visit {ageOfPerson=69, date=DateTime.Now, gender="Male" });
                        //await documentDBUtilities.updateCameraDocument("bBNBAPFVBgAMAAAAAAAAAA==", document);

                    }
                    catch (ClientException ex)
                    {
                        Debug.WriteLine(string.Format("Response: {0}. {1}", ex.Error.Code, ex.Error.Message));
                    }
                }
            }
            else
            {
                MessageDialog msg = new MessageDialog("No internet is avaiable. Please, check your connection.");
                await msg.ShowAsync();
            }
        }

    }
}
