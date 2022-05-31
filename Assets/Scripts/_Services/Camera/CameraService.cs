using Data.Settings;
using System.Linq;
using UnityEngine;
using View;
using Zenject;

namespace Services.Camera
{
    public class CameraService : IFixedTickable
    {
        private readonly SignalBus _signalBus;
        private readonly CameraServiceSettings[] _cameraServiceSettings;

        private CameraServiceSettings _settings;
        private GameObject _baseView;
        private GameObject _cameraView;

        private bool _startProc = false;
        public CameraService(SignalBus signalBus, CameraServiceSettings[] cameraServiceSettings)
        {
            _signalBus = signalBus;
            _cameraServiceSettings = cameraServiceSettings;
        }
        public void ClearServiceValues()
        {
            _startProc = false;
        }

        public void InitializeCamera(string cameraId, IView baseView, IView cameraView)
        {
           _settings = _cameraServiceSettings.FirstOrDefault(cam => cam.Id == cameraId);

            if (_settings != null) 
            {
                switch (_settings.CameraType) 
                {
                    case CameraType.FPSCamera:
                        {
                            FPSCamera(baseView, cameraView, _settings);

                            break;
                        }
                    case CameraType.SideScrollerCamera: 
                        {
                            SideScrollerCamera(baseView, cameraView, _settings);

                            break;
                        }
                    case CameraType.TopDownCamera: 
                        {
                            TopDownCamera(baseView, cameraView, _settings);

                            break;
                        }
                    case CameraType.TPSCamera: 
                        {
                            TPSCamera(baseView, cameraView, _settings);

                            break;
                        }
                }
            }
        }

        public void FixedTick()
        {
            // Maybe FixedUpdate...
            if (_startProc && _baseView != null && _cameraView != null)
            {
                CameraFolow();
            }
            else 
            {

            }
                    
        }

        private void FPSCamera(IView bsView, IView camView, CameraServiceSettings cameraServiceSettings)
        { 
            //ToDo...
        }

        private void SideScrollerCamera(IView bsView, IView camView, CameraServiceSettings cameraServiceSettings) 
        {
            //ToDo...
        }

        private void TopDownCamera(IView bsView, IView camView, CameraServiceSettings cameraServiceSettings) 
        {
           _baseView =  bsView.GetGameObject();
           _cameraView = camView.GetGameObject();

           _cameraView.transform.position = cameraServiceSettings.Position;
           _cameraView.transform.rotation = Quaternion.Euler(cameraServiceSettings.Rotation);

            _startProc = true;
        }

        private void TPSCamera(IView bsView, IView camView, CameraServiceSettings cameraServiceSettings) 
        {
            //ToDo...
        }

        private void CameraFolow() 
        {
            var desiredPosition = _baseView.transform.position + _settings.CameraFollowOffset;
            var smoothedPosition = Vector3.Lerp(_cameraView.transform.position, desiredPosition, _settings.CameraFollowSmoothSpeed);
            _cameraView.transform.position = smoothedPosition + _settings.Position;
        }
       
    }
}