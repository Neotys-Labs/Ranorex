/*
 * Created by Ranorex
 * User: mlasram
 * Date: 24/04/2018
 * Time: 18:11
 * 
 * To change this template use Tools > Options > Coding > Edit standard headers.
 */
using System;
using Neotys.DesignAPI.Model;

namespace MyNeoloadTest.NeoLoad.config
{
	/// <summary>
	/// Description of ParamBuilderProvider.
	/// </summary>
	public class ParamBuilderProvider
    {
        public CloseProjectParamsBuilder newCloseProjectParamsBuilder()
        {
            return new CloseProjectParamsBuilder();
        }

        public StartRecordingParamsBuilder newStartRecordingBuilder()
        {
            return new StartRecordingParamsBuilder();
        }

        public StopRecordingParamsBuilder newStopRecordingBuilder()
        {
            return new StopRecordingParamsBuilder().frameworkParameterSearch(true);
        }

        public UpdateUserPathParamsBuilder newUpdateUserPathParamsBuilder()
        {
            return new UpdateUserPathParamsBuilder().deleteRecording(true);
        }
    }
}
